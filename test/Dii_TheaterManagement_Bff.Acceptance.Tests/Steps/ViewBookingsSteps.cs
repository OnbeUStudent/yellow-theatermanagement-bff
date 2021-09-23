using Dii_TheaterManagement_Bff.Acceptance.Tests.Drivers;
using Dii_TheaterManagement_Bff.Acceptance.Tests.Models;
using Dii_TheaterManagement_Bff.Clients;
using FluentAssertions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using TechTalk.SpecFlow;

namespace Dii_TheaterManagement_Bff.Acceptance.Tests.Steps
{
    [Binding]
    public class ViewBookingsSteps
    {

        private readonly Driver _driver;
        private readonly HttpClient _client;
        private List<Booking> _moviesBooked;
        private int _originalBookingCount;

        public ViewBookingsSteps(Driver driver)
        {
            _driver = driver;
            _client = driver._client;
        }
        [Given(@"list of CurrentBookingsView")]
        public void GivenListOfCurrentBookingsView(Table table)
        {
            _driver.AddBookingsToDatabase(table);
            _originalBookingCount = table.RowCount;
        }
        
        [When(@"I view bookings as an user")]
        public async System.Threading.Tasks.Task WhenIViewBookingsAsAnUserAsync()
        {
            var response = await _client.GetAsync("/api/bookings");

            // Get the response
            var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            Console.WriteLine("Your response data is: " + json);

            // Save the result so we can inspect it later.
            _moviesBooked = JsonConvert.DeserializeObject<List<Booking>>(json);
        }
        
        [Then(@"I am able to see all CurrentBookings")]
        public void ThenIAmAbleToSeeAllCurrentBookings()
        {

            var bookingsCount = _moviesBooked.Count();


            bookingsCount.Should().Equals(_originalBookingCount);
        }
    }
}
