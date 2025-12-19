using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;

namespace STGCLauncher
{
    public static class HttpClientFactory
    {
        public static HttpClient Create()
        {
            var client = new HttpClient
            {
                Timeout = Timeout.InfiniteTimeSpan
            };

            client.DefaultRequestHeaders.ConnectionClose = false;
            client.DefaultRequestHeaders.UserAgent.ParseAdd(
                "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36");
            client.DefaultRequestHeaders.CacheControl = new CacheControlHeaderValue
            {
                NoCache = true,
                NoStore = true
            };

            return client;
        }
    }
}
