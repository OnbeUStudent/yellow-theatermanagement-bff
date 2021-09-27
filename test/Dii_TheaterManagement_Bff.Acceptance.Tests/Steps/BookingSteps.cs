using Dii_TheaterManagement_Bff.Acceptance.Tests.Drivers;
using Dii_TheaterManagement_Bff.Acceptance.Tests.Models;
using Dii_TheaterManagement_Bff.Clients;
using FluentAssertions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace Dii_TheaterManagement_Bff.Acceptance.Tests.Steps
{
    [Binding]
    public class BookingSteps
    {
        private readonly Driver _driver;
        private readonly HttpClient _client;
        private List<Movie> _moviesViewed;

        public BookingSteps(Driver driver)
        {
            _driver = driver;
            _client = driver._client;
        }

        [Given(@"the following movies")]
        public void GivenTheFollowingMovies(Table table)
        {
            _driver.AddMoviesToDatabase(table);
        }
        
        [When(@"I view the list of movies")]
        public async Task WhenIViewTheListOfMoviesAsync()
        {
            var response = await _client.GetAsync("/api/movies");

            // Get the response
            var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            Console.WriteLine("Your response data is: " + json);

            // Save the result so we can inspect it later.
            _moviesViewed = JsonConvert.DeserializeObject<List<Movie>>(json);
        }

        [Then(@"the movie list should show")]
        public void ThenTheMovieListShouldShow(Table table)
        {
            var expectedMovies = table.CreateSet<MovieView>();

            var expectedTitles = expectedMovies.Select(em => em.title);
            var actualTitles = _moviesViewed.Select(am => am.Title);

            actualTitles.Should().Contain(expectedTitles);
        }
    }
}
