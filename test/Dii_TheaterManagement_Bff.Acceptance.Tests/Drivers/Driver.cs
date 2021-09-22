using Dii_TheaterManagement_Bff.Acceptance.Tests.Steps;
using Microsoft.AspNetCore.Mvc.Testing;
using PactTestingTools;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using TechTalk.SpecFlow;
using Xunit;

namespace Dii_TheaterManagement_Bff.Acceptance.Tests.Drivers
{
    public class Driver : IClassFixture<CustomWebApplicationFactory<Startup>>,
        IClassFixture<WebApplicationFactory<Dii_OrderingSvc.Fake.Startup>>
    {


        public HttpClient _client;



        public Driver(CustomWebApplicationFactory<Startup> factory
            , WebApplicationFactory<Dii_OrderingSvc.Fake.Startup> orderServiceFakeFactory
            )
        {
            _client = factory.CreateClient();
            var httpClientForInMemoryInstanceOfOrderingSvcApp = orderServiceFakeFactory.CreateClient();
            var inMemoryReverseProxy_OrderingSvc = new InMemoryReverseProxy(httpClientForInMemoryInstanceOfOrderingSvcApp);
            Startup.OrderingHttpClientBaseAddress = inMemoryReverseProxy_OrderingSvc.LocalhostAddress;
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "e30.eyJzdWIiOiJBNzFDRTAwMC0wMDAwLTAwMDAtMDAwMC0wMDAwMDAwMDAwMDAiLCJuYW1laWQiOiJib3VsZXZhcmRfYWxpY2UiLCJlbWFpbCI6IkFsaWNlQnJvb2tzQGJvdWxldmFyZC50aGUiLCJ1bmlxdWVfbmFtZSI6IkFsaWNlIEJyb29rcyIsIngtdXNlcnR5cGUiOiJyZWFsIiwieC10aGVhdGVyY29kZSI6ImJvdWxldmFyZCIsIm5iZiI6MTYzMTg0ODQ0MSwiZXhwIjozMzE2Nzg0ODQ0MSwiaWF0IjoxNjMxODQ4NDQxLCJpc3MiOiJNeUJhY2tlbmQiLCJhdWQiOiJNeUJhY2tlbmQifQ.");

        }

        internal void AddMoviesToDatabase(Table table)
        {
            // Note that we assume the specified movies are already in the seed data.
        }
    }
}
