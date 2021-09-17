
using Dii_OrderingSvc.Fake;
using Dii_OrderingSvc.Fake.Data;
using Dii_TheaterManagement_Bff.Acceptance.Tests.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using PactTestingTools;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;
using Xunit;

namespace Dii_TheaterManagement_Bff.Acceptance.Tests.Steps
{
    [Binding]
    public class ViewBookingsSteps : IClassFixture<CustomWebApplicationFactory<Startup>>,
        IClassFixture<WebApplicationFactory<Dii_OrderingSvc.Fake.Startup>>
    {
        protected HttpClient _client;
        List<string> names = new List<string>();
        IEnumerable<CreateCurrentBookingsView> createCurrentBookingsView;
        private IEnumerable<Booking> deserialized;

        public ViewBookingsSteps(CustomWebApplicationFactory<Startup> factory
            , WebApplicationFactory<Dii_OrderingSvc.Fake.Startup> orderServiceFakeFactory)
        {
            _client = factory.CreateClient();
            var httpClientForInMemoryInstanceOfOrderingSvcApp = orderServiceFakeFactory.CreateClient();
            var inMemoryReverseProxy_OrderingSvc = new InMemoryReverseProxy(httpClientForInMemoryInstanceOfOrderingSvcApp);
            Startup.OrderingHttpClientBaseAddress = inMemoryReverseProxy_OrderingSvc.LocalhostAddress;
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "e30.eyJzdWIiOiJBNzFDRTAwMC0wMDAwLTAwMDAtMDAwMC0wMDAwMDAwMDAwMDAiLCJuYW1laWQiOiJib3VsZXZhcmRfYWxpY2UiLCJlbWFpbCI6IkFsaWNlQnJvb2tzQGJvdWxldmFyZC50aGUiLCJ1bmlxdWVfbmFtZSI6IkFsaWNlIEJyb29rcyIsIngtdXNlcnR5cGUiOiJyZWFsIiwieC10aGVhdGVyY29kZSI6ImJvdWxldmFyZCIsIm5iZiI6MTYzMTg0ODQ0MSwiZXhwIjozMzE2Nzg0ODQ0MSwiaWF0IjoxNjMxODQ4NDQxLCJpc3MiOiJNeUJhY2tlbmQiLCJhdWQiOiJNeUJhY2tlbmQifQ.");

        }

        //Arange
        [Given(@"list of CurrentBookingsView")]
        public void GivenListOfCurrentBookingsView(Table table)
        {
            createCurrentBookingsView = table.CreateSet<CreateCurrentBookingsView>();
            foreach (CreateCurrentBookingsView descriptor in createCurrentBookingsView)
            {
                names.Add(descriptor.tittle);
            }
        }

        //Act
        [When(@"I view bookings as an admin user on the admin page")]
        public async void WhenIViewBookingsAsAnAdminUserOnTheAdminPage()
        {

            var response = _client.GetAsync("/api/bookings").Result;

            // Get the response
            var bookingsJsonString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            Console.WriteLine("Your response data is: " + bookingsJsonString);

            // Deserialise the data (include the Newtonsoft JSON Nuget package if you don't already have it)
            deserialized = JsonConvert.DeserializeObject<IEnumerable<Booking>>(bookingsJsonString);

        }

        //[When(@"I view bookings as an nonadmin user on the customer page")]
        //public void WhenIViewBookingsAsAnNonadminUserOnTheCustomerPage()
        //{
        //    ScenarioContext.Current.Pending();
        //}

        //Assert
        [Then(@"I am able to see all CurrentBookings")]
        public void ThenIAmAbleToSeeAllCurrentBookings()
        {

            foreach (Booking i in deserialized)
            {
                i.Movie.Title.Should().BeOneOf(names);
            }

        }

        //[Then(@"I cannot see ViewBooking link")]
        //public void ThenICannotSeeViewBookingLink()
        //{
        //    ScenarioContext.Current.Pending();
        //}
    }
}