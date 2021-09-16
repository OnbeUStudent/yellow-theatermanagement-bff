using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Dii_OrderingSvc.Fake.Data
{
    public class DetailedTheater
    {
        [Key]
        [JsonIgnore] // Do not serialize to the API (but do save to database)
        public string TheaterCode { get; set; }

        public string TheaterName { get; set; }

        public string AddressLine1 { get; set; }

        public string AddressLine2 { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public string Zip { get; set; }

        [JsonIgnore] // Do not serialize to the API (but do save to database)
        public ThemeType ThemeType
        {
            get
            {
                return _webSiteTheme.Name == null ? ThemeType.Cerulean : Enum.Parse<ThemeType>(_webSiteTheme.Name);
            }
            set
            {
                _webSiteTheme.Name = Enum.GetName<ThemeType>(value);
            }
        }

        private WebSiteTheme _webSiteTheme = new WebSiteTheme() { Name = Enum.GetName<ThemeType>(ThemeType.Cerulean) };
        [NotMapped] // Do not save to database (but serialize to the API)
        public WebSiteTheme WebSiteTheme
        {
            get
            {
                return _webSiteTheme;
                //return new WebSiteTheme() { Name = Enum.GetName<ThemeType>(ThemeType) };
            }
            set
            {
                _webSiteTheme = value;
            }
        }

        public virtual ICollection<Booking> Bookings { get; set; }
        public virtual ICollection<SnackPackOrder> SnackPackOrders { get; set; }
    }
}
