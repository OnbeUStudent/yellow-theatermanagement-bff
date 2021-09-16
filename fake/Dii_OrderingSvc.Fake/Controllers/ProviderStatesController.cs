using Dii_OrderingSvc.Fake.Data;
using Dii_OrderingSvc.Fake.Features.SeedData;
using Dii_OrderingSvc.Fake.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Dii_OrderingSvc.Fake.Controllers
{
    [Route("provider-states")]
    [ApiController]
    public class ProviderStatesController : ControllerBase
    {
        private readonly OrderingSvcContext orderingSvcContext;

        public ProviderStatesController(OrderingSvcContext orderingSvcContext)
        {
            this.orderingSvcContext = orderingSvcContext;
        }

        // POST provider-states
        [HttpPost]
        public async Task Post()
        {
            ProviderStates providerStates;
            using (StreamReader stream = new StreamReader(HttpContext.Request.Body))
            {
                string body = await stream.ReadToEndAsync();
                providerStates = Newtonsoft.Json.JsonConvert.DeserializeObject<ProviderStates>(body);
            }

            foreach (string state in providerStates.states)
            {
                HandleProviderState(state);
            }
        }

        private void HandleProviderState(string state)
        {
            const string thereAreSomeFakeUsers = "There are SOME fake users";

            const string thereAreNoMovies = "There are NO movies";
            const string thereAreSomeMovies = "There are SOME movies";

            const string theStandardListOfThemes = "The standard list of themes";

            switch (state)
            {
                case thereAreNoMovies:
                    orderingSvcContext.Movies.RemoveRange(orderingSvcContext.Movies);
                    orderingSvcContext.MovieMetadatas.RemoveRange(orderingSvcContext.MovieMetadatas);
                    orderingSvcContext.SaveChanges();
                    return;
                case thereAreSomeMovies:
                    DataSeeding.SeedData(orderingSvcContext);
                    return;
                case thereAreSomeFakeUsers:
                case theStandardListOfThemes:
                    // Do nothing.
                    return;
                default:
                    throw new ArgumentException($"Expected state to be \"{thereAreNoMovies}\" or \"{thereAreSomeMovies}\"");
            }
        }
    }
}
