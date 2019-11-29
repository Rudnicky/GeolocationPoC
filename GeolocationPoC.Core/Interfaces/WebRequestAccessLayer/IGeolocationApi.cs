using GeolocationPoC.Core.Domain;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace GeolocationPoC.Core.Interfaces.WebRequestAccessLayer
{
    public interface IGeolocationApi
    {
        Task<IEnumerable<Geolocation>> GetAll();
        Task<Geolocation> Get(string id);
        Task<string> Post(string ip);
        Task<string> Put(Geolocation geolocation);
        Task<string> Delete(string id);
    }
}
