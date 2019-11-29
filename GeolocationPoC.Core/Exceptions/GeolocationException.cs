using System;
using System.Net;

namespace GeolocationPoC.Core.Exceptions
{
    public class GeolocationException : Exception
    {
        public HttpStatusCode StatusCode { get; set; }
        public string ErrorMessage { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
        public DateTime DateTime { get; set; }

        public GeolocationException(string errorMessage, string controller = "", string action = "", HttpStatusCode statusCode = HttpStatusCode.NotFound) 
        {
            ErrorMessage = errorMessage;
            StatusCode = statusCode;
            Controller = controller;
            Action = action;
            DateTime = DateTime.Now;
        }
    }
}
