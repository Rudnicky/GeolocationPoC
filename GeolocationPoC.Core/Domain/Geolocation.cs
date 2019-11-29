using GeolocationPoC.Core.Domain.Db;
using GeolocationPoC.Core.Domain.Web;

namespace GeolocationPoC.Core.Domain
{
    public class Geolocation : EntityBase
    {
        public string Ip { get; set; }
        public string CountryName { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public Location Location { get; set; }
    }
}
