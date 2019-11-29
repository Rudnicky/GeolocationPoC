using GeolocationPoC.Core.Domain.Web;
using System.Threading.Tasks;

namespace GeolocationPoC.Core.Interfaces.WebRequestAccessLayer
{
    public interface IGeolocationRepository
    {
        Task<IpStack> Get(string ip);
    }
}
