using Dii_TheaterManagement_Bff.Clients;
using PactNet.Matchers;
using PactNet.Mocks.MockHttpService;
using PactNet.Mocks.MockHttpService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Dii_TheaterManagement_Bff.PactConsumer.Tests.DiiOrderingSvc
{
    public class DiiOrderingSvcConsumerPactTests : IClassFixture<DiiOrderingSvcConsumerPactFixture>
    {
        private IMockProviderService _mockProviderService;
        private string _mockProviderServiceBaseUri;

        public DiiOrderingSvcConsumerPactTests(DiiOrderingSvcConsumerPactFixture fixture)
        {
            _mockProviderService = fixture.MockProviderService;
            _mockProviderService.ClearInteractions(); //NOTE: Clears any previously registered interactions before the test is run
            _mockProviderServiceBaseUri = fixture.MockProviderServiceBaseUri;

        }

        [Fact]
        public async Task ClientForOrderingSvc_Movies_GivenSomeMovies()
        {
            // Arrange
            _mockProviderService
              .Given("There are SOME movies")
              .UponReceiving("A GET request to retrieve the list of movies")
              .With(new ProviderServiceRequest
              {
                  Method = HttpVerb.Get,
                  Path = "/api/movies"


              })
              .WillRespondWith(new ProviderServiceResponse
              {
                  Status = 200,
                  Headers = new Dictionary<string, object>
                  {
                      { "Content-Type", "application/json; charset=utf-8" }
                  },
                  Body = Match.MinType(new { MovieId = 1, Title = "Example Movie" }, 1)
              });
            var httpClient = new HttpClient { BaseAddress = new Uri(_mockProviderServiceBaseUri) };
            httpClient.DefaultRequestHeaders
              .Accept
              .Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var orderingSvcClient = new OrderingSvcClient(httpClient);

            // Act
            var result = await orderingSvcClient.ApiMoviesGetAsync();

            // Assert
            Assert.True(result != null && result.Count > 0);


            _mockProviderService.VerifyInteractions(); //NOTE: Verifies that interactions registered on the mock provider are called at least once
        }
    }
}
