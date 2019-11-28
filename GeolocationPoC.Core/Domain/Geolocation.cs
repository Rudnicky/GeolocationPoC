using GeolocationPoC.Core.Domain.Db;
using GeolocationPoC.Core.Domain.Web;
using System.ComponentModel.DataAnnotations;

namespace GeolocationPoC.Core.Domain
{
    public class Geolocation : EntityBase
    {
        [Key]
        public int Id { get; set; } 
        public string Ip { get; set; }
        public string CountryName { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public Location Location { get; set; }
    }
}
