using System.Net.Http;

namespace Dii_TheaterManagement_Bff.Clients.MovieCatalogSvc
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
