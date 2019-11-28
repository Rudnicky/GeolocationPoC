using System;

namespace GeolocationPoC.Core.Exceptions
{
    public class ServiceAuthenticationEx : Exception
    {
        public string Content { get; }

        public ServiceAuthenticationEx()
        {
        }

        public ServiceAuthenticationEx(string content)
        {
            Content = content;
        }
    }
}
