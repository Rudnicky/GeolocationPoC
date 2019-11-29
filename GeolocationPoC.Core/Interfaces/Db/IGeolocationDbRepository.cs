using GeolocationPoC.Core.Domain;
using System.Threading.Tasks;

namespace GeolocationPoC.Core.Interfaces.Db
{
    public interface IGeolocationDbRepository : IRepository<Geolocation>
    {
        Task<Geolocation> FindByIp(string ip);
    }
}
