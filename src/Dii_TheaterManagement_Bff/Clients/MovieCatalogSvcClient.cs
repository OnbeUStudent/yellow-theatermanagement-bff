using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Dii_TheaterManagement_Bff.Clients
{
    public class MovieCatalogSvcClient
    {
        private readonly HttpClient httpClient;

        public MovieCatalogSvcClient(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }
    }
}
