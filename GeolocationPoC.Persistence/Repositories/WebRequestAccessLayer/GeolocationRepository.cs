using GeolocationPoC.Core.Domain.Web;
using GeolocationPoC.Core.Interfaces.WebRequestAccessLayer;
using GeolocationPoC.Core.Utils;
using System.Threading.Tasks;

namespace GeolocationPoC.Persistence.Repositories.WebRequestAccessLayer
{
    public class GeolocationRepository : IGeolocationRepository
    {
        private readonly IRequestProvider _requestProvider;

        public GeolocationRepository(IRequestProvider requestProvider)
        {
            _requestProvider = requestProvider;
        }

        public async Task<IpStack> Get(string ip)
        {
            var url = $"{Constants.BASE_ENDPOINT}/{ip}";
            return await _requestProvider.GetAsync<IpStack>(url, Constants.API_KEY);
        }
    }
}
