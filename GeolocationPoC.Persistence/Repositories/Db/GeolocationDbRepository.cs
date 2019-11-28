using GeolocationPoC.Core.Domain;
using GeolocationPoC.Core.Interfaces.Db;

namespace GeolocationPoC.Persistence.Repositories.Db
{
    public class GeolocationDbRepository : Repository<Geolocation>, IGeolocationDbRepository
    {
        public GeolocationDbRepository(GeolocationDbContext context) : base(context)
        {
        }
    }
}
