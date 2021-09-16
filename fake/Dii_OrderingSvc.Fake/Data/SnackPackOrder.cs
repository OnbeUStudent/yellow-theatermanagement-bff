using System.Text.Json.Serialization;

namespace Dii_OrderingSvc.Fake.Data
{
    public class SnackPackOrder
    {
        /// <summary>
        /// One of two keys that make up <see cref="SnackPackOrder"/>'s composite key.
        /// </summary>
        public string TheaterCode { get; set; }
        [JsonIgnore] // Do not serialize to the API (but do save to database)
        public DetailedTheater DetailedTheater { get; set; }

        /// <summary>
        /// One of two keys that make up <see cref="SnackPackOrder"/>'s composite key.
        /// </summary>
        public int MonthId { get; set; }

        public int OrderCount { get; set; }
    }
}
