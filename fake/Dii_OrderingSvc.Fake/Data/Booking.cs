using System;
using System.Text.Json.Serialization;

namespace Dii_OrderingSvc.Fake.Data
{
    public class Booking
    {
        public Booking()
        {

        }
        public Booking(string theaterCode, int monthId, Guid movieId)
        {
            TheaterCode = theaterCode;
            MonthId = monthId;
            MovieId = movieId;
        }

        public Guid MovieId { get; set; }
        public Movie Movie { get; set; }

        /// <summary>
        /// One of two keys that make up <see cref="Booking"/>'s composite key.
        /// </summary>
        public string TheaterCode { get; set; }
        [JsonIgnore] // Do not serialize to the API (but do save to database)
        public DetailedTheater DetailedTheater { get; set; }

        /// <summary>
        /// One of two keys that make up <see cref="Booking"/>'s composite key.
        /// </summary>
        public int MonthId { get; set; }
    }
}
