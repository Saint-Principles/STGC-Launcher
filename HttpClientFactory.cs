using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;

namespace STGCLauncher
{
    public static class HttpClientFactory
    {
        public static HttpClient Create()
        {
            var handler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true,
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate,
                UseCookies = false,
                AllowAutoRedirect = true,
                MaxAutomaticRedirections = 3
            };

            var client = new HttpClient(handler)
            {
                Timeout = TimeSpan.FromSeconds(30)
            };

            client.DefaultRequestHeaders.UserAgent.ParseAdd(
                "STGCLauncher/1.0 (Windows NT 10.0; Win64; x64)");

            client.DefaultRequestHeaders.Accept.ParseAdd("text/plain,text/html,*/*");
            client.DefaultRequestHeaders.AcceptEncoding.ParseAdd("gzip, deflate");
            client.DefaultRequestHeaders.Connection.ParseAdd("keep-alive");
            client.DefaultRequestHeaders.CacheControl = new CacheControlHeaderValue
            {
                NoCache = true
            };

            return client;
        }
    }
}
