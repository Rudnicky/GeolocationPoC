using GeolocationPoC.Core.Domain.Web;
using System.Threading.Tasks;

namespace GeolocationPoC.Core.Interfaces.Web
{
    public interface IGeolocationRepository
    {
        Task<IpStack> Get(string ip);
    }
}
