using System.Net;

namespace GeolocationPoC.Core.Domain.Db
{
    public class GeolocationResult
    {
        public HttpStatusCode StatusCode { get; set; }
        public string Message { get; set; }
    }
}
