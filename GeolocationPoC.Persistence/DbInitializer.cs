using GeolocationPoC.Core.Domain;
using System.Collections.Generic;
using System.Linq;

namespace GeolocationPoC.Persistence
{
    public class DbInitializer
    {
        public static void Initialize(GeolocationDbContext context)
        {
            // EnsureCreated will cause the database to be created
            // whenever it's needed to be. If it's already there
            // it won't do anything
            context.Database.EnsureCreated();

            // Check if specified table has any data in it
            // if not, then create some dummy data 
            if (context.Geolocations.Any())
            {
                return;
            }

            // Create loads of Dummy Data
            var geolocations = new List<Geolocation>()
            {
                new Geolocation() { Ip = "35.32.112.196", CountryName = "Poland", Longitude = 234.55555, Latitude = 645.99999 },
                new Geolocation() { Ip = "20.36.112.196", CountryName = "Poland", Longitude = 234.55555, Latitude = 645.99999 },
                new Geolocation() { Ip = "15.37.112.196", CountryName = "Poland", Longitude = 234.55555, Latitude = 645.99999 },
                new Geolocation() { Ip = "88.39.112.196", CountryName = "Poland", Longitude = 234.55555, Latitude = 645.99999 },
            };

            geolocations.ForEach(x => context.Geolocations.Add(x));

            context.SaveChanges();
        }
    }
}
