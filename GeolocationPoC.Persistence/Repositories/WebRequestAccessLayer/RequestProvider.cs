using GeolocationPoC.Core.Exceptions;
using GeolocationPoC.Core.Interfaces.WebRequestAccessLayer;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace GeolocationPoC.Persistence.Repositories.WebRequestAccessLayer
{
    public class RequestProvider : IRequestProvider
    {
        private readonly JsonSerializerSettings _serializerSettings;

        public RequestProvider()
        {
            _serializerSettings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                DateTimeZoneHandling = DateTimeZoneHandling.Utc,
                NullValueHandling = NullValueHandling.Ignore
            };
            _serializerSettings.Converters.Add(new StringEnumConverter());
        }

        public async Task<TResult> GetAsync<TResult>(string uri, string token)
        {
            // ugly hack for overstepping ssl
            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };

            HttpClient httpClient = CreateHttpClient(clientHandler);
            HttpResponseMessage response = null;

            if (!string.IsNullOrEmpty(token))
            {
                response = await httpClient.GetAsync($"{uri}?access_key={token}");
            }
            else
            {
                response = await httpClient.GetAsync(uri);
            }

            await HandleResponse(response);
            string serialized = await response.Content.ReadAsStringAsync();

            TResult result = await Task.Run(() => JsonConvert.DeserializeObject<TResult>(serialized, _serializerSettings));

            return result;
        }

        public async Task<TResult> Delete<TResult>(string uri)
        {
            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };

            HttpClient httpClient = CreateHttpClient(clientHandler);
            HttpResponseMessage response = await httpClient.DeleteAsync(uri);

            await HandleResponse(response);
            string serialized = await response.Content.ReadAsStringAsync();

            TResult result = await Task.Run(() => JsonConvert.DeserializeObject<TResult>(serialized, _serializerSettings));

            return result;
        }

        public async Task<string> PostAsync(string uri)
        {
            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };

            HttpClient httpClient = CreateHttpClient(clientHandler);
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, uri);

            var response = await httpClient.SendAsync(request).ConfigureAwait(false);

            await HandleResponse(response);

            return response.StatusCode.ToString();
        }

        public async Task<string> PutAsync(string uri, string json)
        {
            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };

            HttpClient httpClient = CreateHttpClient(clientHandler);

            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await httpClient.PutAsync(uri, content);

            await HandleResponse(response);

            return response.StatusCode.ToString();
        }

        private HttpClient CreateHttpClient(HttpClientHandler clientHandler)
        {
            var httpClient = new HttpClient(clientHandler);

            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            return httpClient;
        }

        private async Task HandleResponse(HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();

                if (response.StatusCode == HttpStatusCode.Forbidden ||
                    response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    throw new ServiceAuthenticationEx(content);
                }

                throw new HttpRequestExceptionEx(response.StatusCode, content);
            }
        }
    }
}
