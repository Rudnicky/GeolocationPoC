using GeolocationPoC.Core.Domain;
using Microsoft.EntityFrameworkCore;

namespace GeolocationPoC.Persistence
{
    public class GeolocationDbContext : DbContext
    {
        public GeolocationDbContext(DbContextOptions<GeolocationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Geolocation> Geolocations { get; set; }
    }
}
