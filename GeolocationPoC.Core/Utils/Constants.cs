namespace GeolocationPoC.Core.Utils
{
    public static class Constants
    {
        public const string API_KEY = "0db44c34634d04293f819a75c6dc4ae4";

        public const string BASE_ENDPOINT = "http://api.ipstack.com";

        public const string LOCAL_ENDPOINT = "https://localhost:5001/api/geolocation";

        public const string IP_REGEX_PATTERN = @"^((25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.){3}(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)$";
    }
}
