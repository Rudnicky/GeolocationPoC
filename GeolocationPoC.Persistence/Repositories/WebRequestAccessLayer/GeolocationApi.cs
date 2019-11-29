using GeolocationPoC.Core.Domain;
using GeolocationPoC.Core.Interfaces.WebRequestAccessLayer;
using GeolocationPoC.Core.Utils;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace GeolocationPoC.Persistence.Repositories.WebRequestAccessLayer
{
    public class GeolocationApi : IGeolocationApi
    {
        private readonly IRequestProvider _requestProvider;

        public GeolocationApi(IRequestProvider requestProvider)
        {
            _requestProvider = requestProvider;
        }

        public async Task<string> Delete(string id)
        {
            var url = $"{Constants.LOCAL_ENDPOINT}/delete/{id}";
            return await _requestProvider.Delete<string>(url);
        }

        public async Task<Geolocation> Get(string id)
        {
            var url = $"{Constants.LOCAL_ENDPOINT}/{id}";
            return await _requestProvider.GetAsync<Geolocation>(url);
        }

        public async Task<IEnumerable<Geolocation>> GetAll()
        {
            var url = $"{Constants.LOCAL_ENDPOINT}/all";
            return await _requestProvider.GetAsync<IEnumerable<Geolocation>>(url);
        }

        public async Task<string> Post(string ip)
        {
            var url = $"{Constants.LOCAL_ENDPOINT}/create/{ip}";
            return await _requestProvider.PostAsync(url);
        }

        public async Task<string> Put(Geolocation geolocation)
        {
            var url = $"{Constants.LOCAL_ENDPOINT}/update";
            var json = JsonSerializer.Serialize(geolocation);

            return await _requestProvider.PutAsync(url, json);
        }
    }
}
