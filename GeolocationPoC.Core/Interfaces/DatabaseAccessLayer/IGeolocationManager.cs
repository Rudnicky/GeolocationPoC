using GeolocationPoC.Core.Domain;
using GeolocationPoC.Core.Domain.Db;
using System.Threading.Tasks;

namespace GeolocationPoC.Core.Interfaces.DatabaseAccessLayer
{
    public interface IGeolocationManager
    {
        Task<GeolocationResult> Update(Geolocation geolocation);
        Task<GeolocationResult> Save(string ip);
        Task<GeolocationResult> Delete(string id);
    }
}
