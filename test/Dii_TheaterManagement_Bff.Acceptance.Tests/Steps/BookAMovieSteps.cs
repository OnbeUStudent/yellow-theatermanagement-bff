using FluentAssertions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Net;
using System.Threading.Tasks;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;
using Dii_TheaterManagement_Bff.Acceptance.Tests.Models;

using Xunit;
using Microsoft.AspNetCore.Mvc.Testing;
using PactTestingTools;
using Dii_TheaterManagement_Bff.Clients;
using Dii_TheaterManagement_Bff.Acceptance.Tests.Drivers;

namespace Dii_TheaterManagement_Bff.Acceptance.Tests.Steps
{
    [Binding]
    public class BookAMovieSteps : IClassFixture<CustomWebApplicationFactory<Startup>>,
        IClassFixture<WebApplicationFactory<Dii_OrderingSvc.Fake.Startup>>
    {
        private readonly ScenarioContext _scenarioContext;
        IEnumerable<CreateCurrentBookingsView> createCurrentBookingsView;
        protected HttpClient _client;
        List<string> names = new List<string>();
        string statusCodeString;
        string bookingTittle, bookingDate;
        HttpResponseMessage response;
        string responseContent;


        public BookAMovieSteps(ScenarioContext scenarioContext,
            CustomWebApplicationFactory<Startup> factory
            , WebApplicationFactory<Dii_OrderingSvc.Fake.Startup> orderServiceFakeFactory
            )
        {
            _scenarioContext = scenarioContext;
            _client = factory.CreateClient();
            var httpClientForInMemoryInstanceOfOrderingSvcApp = orderServiceFakeFactory.CreateClient();
            var inMemoryReverseProxy_OrderingSvc = new InMemoryReverseProxy(httpClientForInMemoryInstanceOfOrderingSvcApp);
            Startup.OrderingHttpClientBaseAddress = inMemoryReverseProxy_OrderingSvc.LocalhostAddress;
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "e30.eyJzdWIiOiJBNzFDRTAwMC0wMDAwLTAwMDAtMDAwMC0wMDAwMDAwMDAwMDAiLCJuYW1laWQiOiJib3VsZXZhcmRfYWxpY2UiLCJlbWFpbCI6IkFsaWNlQnJvb2tzQGJvdWxldmFyZC50aGUiLCJ1bmlxdWVfbmFtZSI6IkFsaWNlIEJyb29rcyIsIngtdXNlcnR5cGUiOiJyZWFsIiwieC10aGVhdGVyY29kZSI6ImJvdWxldmFyZCIsIm5iZiI6MTYzMTg0ODQ0MSwiZXhwIjozMzE2Nzg0ODQ0MSwiaWF0IjoxNjMxODQ4NDQxLCJpc3MiOiJNeUJhY2tlbmQiLCJhdWQiOiJNeUJhY2tlbmQifQ.");

        }


        [Given(@"list of CurrentBookingsBooked")]
        public void GivenListOfCurrentBookingsBooked(Table currentBookings)
        {
            createCurrentBookingsView = currentBookings.CreateSet<CreateCurrentBookingsView>();
            foreach (CreateCurrentBookingsView descriptor in createCurrentBookingsView)
            {
                names.Add(descriptor.tittle);
            }
        }

        //Arrange
        [Given(@"I want to book (.*) on date (.*)")]
        public void WhenIWantToBookThisCurrentBookingsOnThisDate(string tittle, string date)
        {
            bookingTittle = tittle;
            bookingDate = date;
        }

        //Act
        [When(@"I am booking as an (.*) user")]
        public async Task WhenIAmATypeOfUserAsync(String userType)
        {
            //_httpClient = _factory.CreateClient();
            //if (userType == "admin")
            //    _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "e30.eyJzdWIiOiJBNzFDRTAwMC0wMDAwLTAwMDAtMDAwMC0wMDAwMDAwMDAwMDAiLCJuYW1laWQiOiJib3VsZXZhcmRfYWxpY2UiLCJlbWFpbCI6IkFsaWNlQnJvb2tzQGJvdWxldmFyZC50aGUiLCJ1bmlxdWVfbmFtZSI6IkFsaWNlIEJyb29rcyIsIngtdXNlcnR5cGUiOiJyZWFsIiwieC10aGVhdGVyY29kZSI6ImJvdWxldmFyZCIsIm5iZiI6MTYzMTMxMDQ3MSwiZXhwIjozMzE2NzMxNDA3MSwiaWF0IjoxNjMxMzEwNDcxLCJpc3MiOiJNeUJhY2tlbmQiLCJhdWQiOiJNeUJhY2tlbmQifQ.");

            #region new object Booking
            Booking newBooking = new Booking
            {
                //MovieId = 20,
                Movie = new Movie
                {
                    //MovieId = 20,
                    Bookings = null,
                    MovieMetadata = new MovieMetadata
                    {
                        Title = bookingTittle,
                        ImdbId = "tt0086190",
                        Year = "1983",
                        Rated = "PG",
                        Released = "25 May 1983",
                        Runtime = "131 min",
                        Genre = "Action, Adventure, Fantasy, Sci-Fi",
                        Director = "Richard Marquand",
                        Writer = "Lawrence Kasdan (screenplay by), George Lucas (screenplay by), George Lucas (story by)",
                        Actors = "Mark Hamill, Harrison Ford, Carrie Fisher, Billy Dee Williams",
                        Plot = "After a daring mission to rescue Han Solo from Jabba the Hutt, the Rebels dispatch to Endor to destroy the second Death Star. Meanwhile, Luke struggles to help Darth Vader back from the dark side without falling into the Emperor's trap.",
                        Language = "English",
                        Country = "USA",
                        Awards = "Nominated for 4 Oscars. Another 22 wins & 16 nominations.",
                        Poster = null,
                        Metascore = "58",
                        ImdbRating = "8.3",
                        ImdbVotes = "947,486",
                        Type = "movie",
                        TotalSeasons = 0,
                        Response = "True"
                    },
                    Title = bookingTittle
                },
                TheaterCode = "boulevard",
                MonthId = Int32.Parse(bookingDate)

            };
            #endregion

            string jsonString = JsonConvert.SerializeObject(newBooking);
            HttpContent httpContent = new StringContent(jsonString);
            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            //StringContent httpContent = new StringContent(newBooking.ToString(), System.Text.Encoding.UTF8, "application/json");
            //response = await _client.GetAsync("/api/theaters/boulevard/bookings");
            response = await _client.PutAsync("/api/bookings/" + Int32.Parse(bookingDate), httpContent).ConfigureAwait(false);

            // Get the response
            //statusCodeString = response.StatusCode.ToString();
            Console.WriteLine("Your response data is: " + response);

            responseContent = response.Content.ReadAsStringAsync().Result;

        }


        //[When(@"CurrentBookings does not already exist for this date")]
        //public void WhenMovieDoesNotAlreadyExistForThisDate()
        //{
        //    //ScenarioContext.Current.Pending();
        //}

        //[When(@"I want to book this \[movie] on this \[date]")]
        //public void WhenIWantToBookThisMovieOnThisDate()
        //{
        //    //ScenarioContext.Current.Pending();
        //}

        //Assert
        [Then(@"the booking will be created")]
        public void ThenTheBookingWillBeCreated()
        {
            response.IsSuccessStatusCode.Should().Be(true);
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        //Assert
        [Then(@"the booking will not be created")]
        public void ThenTheBookingWillNotBeCreated()
        {
            response.IsSuccessStatusCode.Should().Be(false);
            responseContent.Should().Contain("Authorization header is required");
        }
    }
}