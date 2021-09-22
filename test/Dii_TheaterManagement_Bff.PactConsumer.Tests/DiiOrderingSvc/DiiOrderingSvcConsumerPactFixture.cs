using PactNet;
using PactNet.Mocks.MockHttpService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dii_TheaterManagement_Bff.PactConsumer.Tests.DiiOrderingSvc
{
  
    // This class is responsible for setting up a shared
    // mock server for Pact used by all the tests.
    // XUnit can use a Class Fixture for this.
    public class DiiOrderingSvcConsumerPactFixture : IDisposable
    {
        public IPactBuilder PactBuilder { get; private set; }
        public IMockProviderService MockProviderService { get; private set; }

        public int MockServerPort { get { return 9224; } }
        public string MockProviderServiceBaseUri { get { return String.Format("http://localhost:{0}", MockServerPort); } }

        public DiiOrderingSvcConsumerPactFixture()
        {
            var pactConfig = new PactConfig
            {
                SpecificationVersion = "2.0.0",
                PactDir = @".\..\..\..\..\..\pacts\contracts",
                LogDir = @".\..\..\..\..\..\pacts\logs"
            };

            //PactBuilder = new PactBuilder(new PactConfig { SpecificationVersion = "2.0.0" }); //Configures the Specification Version

            PactBuilder = new PactBuilder(pactConfig);

            PactBuilder.ServiceConsumer("white-theatermanagement-bff")
                       .HasPactWith("white-ordering-svc");

            MockProviderService = PactBuilder.MockService(MockServerPort);
        }


        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // This will save the pact file once finished.
                    PactBuilder.Build();
                }

                disposedValue = true;
            }
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
        }
        #endregion
    }
}
