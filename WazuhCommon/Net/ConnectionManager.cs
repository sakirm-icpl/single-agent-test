namespace WazuhCommon.Net
{
    public class ConnectionManager
    {
        private static readonly Lazy<HttpClient> lazyClient = new Lazy<HttpClient>(CreateHttpClient);

        public static HttpClient Client => lazyClient.Value;

        private static HttpClient CreateHttpClient()
        {
            var handler = new HttpClientHandler
            {
                // Uncomment the following line ONLY for development purposes where SSL validation bypass is necessary.
                ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
            };

            return new HttpClient(new CustomHttpHandler(handler, 10));
        }
    }
}
