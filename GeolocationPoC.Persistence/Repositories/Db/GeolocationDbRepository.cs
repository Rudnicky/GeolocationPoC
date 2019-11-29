using GeolocationPoC.Core.Domain;
using GeolocationPoC.Core.Interfaces.Db;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace GeolocationPoC.Persistence.Repositories.Db
{
    public class GeolocationDbRepository : Repository<Geolocation>, IGeolocationDbRepository
    {
        public GeolocationDbRepository(GeolocationDbContext context) : base(context)
        {
        }

        public async Task<Geolocation> FindByIp(string ip)
        {
            return await Context.Set<Geolocation>().SingleOrDefaultAsync(x => x.Ip == ip).ConfigureAwait(false);
        }
    }
}
