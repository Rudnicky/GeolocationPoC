namespace GeolocationPoC.Core.Utils
{
    public static class ErrorMessages
    {
        public const string WRONG_ID = "Id must be specified and not equal zero";

        public const string WRONG_IP = "Ip must be specified";

        public const string WRONG_ID_FORMAT = "Id has wrong format";

        public const string WRONG_IP_PATTERN = "Ip has a wrong pattern. Example: [15.37.112.196]";

        public const string EXISTS = "Object already exists";

        public const string NOT_EXISTS = "Object deos not exists. Please verify your id";

        public const string IP_STACK_ERROR = "Something went wrong during getting data through https://ipstack.com/ api";

        public const string CREATED = "Object Created Successfully!";

        public const string UPDATED = "Object Updated Successfully!";

        public const string DELETED = "Object Deleted Successfully!";

        public const string DB_ERROR = "Probably something went wrong during adding object to the database";
    }
}
