using GeolocationPoC.Core.Domain;
using GeolocationPoC.Core.Domain.Db;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GeolocationPoC.Core.Interfaces.WebRequestAccessLayer
{
    public interface IGeolocationApi
    {
        Task<IEnumerable<Geolocation>> GetAll();
        Task<Geolocation> Get(string id);
        Task Post(string ip);
        Task Put(Geolocation geolocation);
        Task<string> Delete(string id);
    }
}
