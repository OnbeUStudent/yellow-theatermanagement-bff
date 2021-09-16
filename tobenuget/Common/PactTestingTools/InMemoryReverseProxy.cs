using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Threading;

namespace PactTestingTools
{
    // Based on code from:
    //  - https://docs.microsoft.com/en-us/dotnet/api/system.net.httplistener.begingetcontext?view=net-5.0
    //  - https://auth0.com/blog/building-a-reverse-proxy-in-dot-net-core/
    //  - https://stackoverflow.com/questions/4672010/multi-threading-with-net-httplistener/44229345#44229345

    /// <summary>
    /// A reverse proxy that's useful when your automated test require you to expose an HTTP server
    /// out-of-process but you want to use Microsoft.AspNetCore.Mvc.Testing's WebApplicationFactory.CreateClient() 
    /// method to test your application in-process.
    /// 
    /// The constructor accepts your in-process HttpClient then establishes an HttpListener which listens on the 
    /// first open port it can find; e.g. http://localhost:12345.
    /// 
    /// Then (until the reverse proxy is disposed via IDisposable) the reverse proxy will listen for any calls
    /// on that port and will forward them to the in-process HttpClient.
    /// 
    /// Be sure to employ a using block, like this:
    /// <code>
    ///    var httpClientForInMemoryInstanceOfApp = _factory.CreateClient();
    ///    using (var inMemoryReverseProxy = new InMemoryReverseProxy(httpClientForInMemoryInstanceOfApp))
    ///    {
    ///       var httpClient = new HttpClient() { BaseAddress = new Uri(inMemoryReverseProxy.LocalhostAddress) };
    ///       ... Now run your test, calling httpClient instead of httpClientForInMemoryInstanceOfApp
    ///    }
    ///    </code>
    /// </summary>
    public class InMemoryReverseProxy : IDisposable
    {
        private readonly HttpClient _httpClient;
        private readonly HttpListener _listener;
        private readonly Thread _listenerThread;
        private readonly Thread[] _workers;
        private readonly ManualResetEvent _stop, _ready;
        private Queue<HttpListenerContext> _queue;

        /// <summary>
        /// The port that's being listened upon; e.g. 12345
        /// </summary>
        public int Port { get; }
        /// <summary>
        /// The address that's being listened upon; e.g. "http://localhost:12345"
        /// </summary>
        public string LocalhostAddress { get; }

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="httpClient">The in-process HttpClient to which requests will be forwarded.</param>
        /// <param name="maxThreads">The maximum number of request-forwarding threads. Defaults to 1.</param>
        public InMemoryReverseProxy(HttpClient httpClient, int maxThreads = 1)
        {
            _httpClient = httpClient;
            _workers = new Thread[maxThreads];
            _queue = new Queue<HttpListenerContext>();
            _stop = new ManualResetEvent(false);
            _ready = new ManualResetEvent(false);

            Port = ReserveAnOpenLocalhostPort();
            LocalhostAddress = $"http://localhost:{Port}";

            string localhostAddressWithSlashAtEnd = $"http://localhost:{Port}/";
            _listener = new HttpListener();
            _listener.Prefixes.Add(localhostAddressWithSlashAtEnd);
            _listener.Start();

            _listenerThread = new Thread(HandleRequests);
            _listenerThread.Start();

            for (int i = 0; i < _workers.Length; i++)
            {
                _workers[i] = new Thread(Worker);
                _workers[i].Start();
            }

            // IAsyncResult result = _listener.BeginGetContext(new AsyncCallback(ListenerCallback), _listener);
        }

        private void Stop()
        {
            _stop.Set();
            _listenerThread.Join();
            foreach (Thread worker in _workers)
                worker.Join();
            _listener.Stop();
        }

        private void HandleRequests()
        {
            while (_listener.IsListening)
            {
                var context = _listener.BeginGetContext(ContextReady, null);

                if (0 == WaitHandle.WaitAny(new[] { _stop, context.AsyncWaitHandle }))
                    return;
            }
        }

        private void ContextReady(IAsyncResult ar)
        {
            try
            {
                lock (_queue)
                {
                    _queue.Enqueue(_listener.EndGetContext(ar));
                    _ready.Set();
                }
            }
            catch { return; }
        }

        private void Worker()
        {
            WaitHandle[] wait = new[] { _ready, _stop };
            while (0 == WaitHandle.WaitAny(wait))
            {
                HttpListenerContext context;
                lock (_queue)
                {
                    if (_queue.Count > 0)
                        context = _queue.Dequeue();
                    else
                    {
                        _ready.Reset();
                        continue;
                    }
                }

                try { ProcessRequest(context); }
                catch (Exception e) { Console.Error.WriteLine(e); }
            }
        }

        internal static ConcurrentDictionary<int, int> _reservedPorts = new ConcurrentDictionary<int, int>();

        internal static int ReserveAnOpenLocalhostPort()
        {
            bool added = false;
            int port;
            do
            {
                var listener = new TcpListener(IPAddress.Loopback, 0);
                listener.Start();
                port = ((IPEndPoint)listener.LocalEndpoint).Port;
                listener.Stop();

                if (_reservedPorts.TryAdd(port, port))
                {
                    added = true;
                }
            } while (!added);
            return port;
        }

        internal static void UnreserveAPort(int port)
        {
            _ = _reservedPorts.TryRemove(new KeyValuePair<int, int>(port, port));
        }

        private void ProcessRequest(HttpListenerContext context)
        {
            HttpListenerRequest request = context.Request;
            HttpListenerResponse response = context.Response;

            var targetRequestMessage = CreateTargetMessage(context, context.Request.Url);

            using (var responseMessage = _httpClient.SendAsync(targetRequestMessage, HttpCompletionOption.ResponseHeadersRead).Result)
            {
                context.Response.StatusCode = (int)responseMessage.StatusCode;
                CopyFromTargetResponseHeaders(context, responseMessage);

                byte[] buffer = responseMessage.Content.ReadAsByteArrayAsync().Result;
                // Get a response stream and write the response to it.
                response.ContentLength64 = buffer.Length;
                response.StatusCode = (int)responseMessage.StatusCode;
                System.IO.Stream output = response.OutputStream;
                output.Write(buffer, 0, buffer.Length);
                output.Close();
            }
        }

        private static HttpRequestMessage CreateTargetMessage(HttpListenerContext context, Uri targetUri)
        {
            var requestMessage = new HttpRequestMessage();
            CopyFromOriginalRequestContentAndHeaders(context, requestMessage);

            requestMessage.RequestUri = targetUri;
            requestMessage.Headers.Host = targetUri.Host;
            requestMessage.Method = GetMethod(context.Request.HttpMethod);

            return requestMessage;
        }

        private static void CopyFromOriginalRequestContentAndHeaders(HttpListenerContext context, HttpRequestMessage requestMessage)
        {
            var requestMethod = context.Request.HttpMethod;

            if (!HttpMethods.IsGet(requestMethod) &&
              !HttpMethods.IsHead(requestMethod) &&
              !HttpMethods.IsDelete(requestMethod) &&
              !HttpMethods.IsTrace(requestMethod))
            {
                var streamContent = new StreamContent(context.Request.InputStream);
                requestMessage.Content = streamContent;
            }

            foreach (var key in context.Request.Headers.AllKeys)
            {
                requestMessage.Content?.Headers.TryAddWithoutValidation(key, context.Request.Headers[key]);
            }
        }

        private static void CopyFromTargetResponseHeaders(HttpListenerContext context, HttpResponseMessage responseMessage)
        {
            // var x = context.Response.Headers..First();
            foreach (var header in responseMessage.Headers)
            {
                context.Response.Headers[header.Key] = header.Value.First();
            }

            foreach (var header in responseMessage.Content.Headers)
            {
                context.Response.Headers[header.Key] = header.Value.First();
            }
            context.Response.Headers.Remove("transfer-encoding");
        }

        private static HttpMethod GetMethod(string method)
        {
            if (HttpMethods.IsDelete(method)) return HttpMethod.Delete;
            if (HttpMethods.IsGet(method)) return HttpMethod.Get;
            if (HttpMethods.IsHead(method)) return HttpMethod.Head;
            if (HttpMethods.IsOptions(method)) return HttpMethod.Options;
            if (HttpMethods.IsPost(method)) return HttpMethod.Post;
            if (HttpMethods.IsPut(method)) return HttpMethod.Put;
            if (HttpMethods.IsTrace(method)) return HttpMethod.Trace;
            return new HttpMethod(method);
        }

        #region IDisposable
        private bool _disposedValue;
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    Stop();
                    UnreserveAPort(Port);
                }

                _disposedValue = true;
            }
        }

        void IDisposable.Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
