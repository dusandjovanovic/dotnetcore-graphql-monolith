namespace GraphQL.API.Constants
{
     public static class OpenTelemetryAttributeName
    {
        public static class Deployment
        {
            public const string Environment = "deployment.environment";
        }
        
        public static class EndUser
        {
            public const string Id = "enduser.id";
            public const string Role = "enduser.role";
            public const string Scope = "enduser.scope";
        }
        
        public static class Http
        {
            public const string Scheme = "http.scheme";
            public const string Flavor = "http.flavor";
            public const string ClientIP = "http.client_ip";
            public const string RequestContentLength = "http.request_content_length";
            public const string RequestContentType = "http.request_content_type";
            public const string ResponseContentLength = "http.response_content_length";
            public const string ResponseContentType = "http.response_content_type";
        }
        
        public static class Host
        {
            public const string Name = "host.name";
        }
        
        public static class Service
        {
            public const string Name = "service.name";
        }
    }
}