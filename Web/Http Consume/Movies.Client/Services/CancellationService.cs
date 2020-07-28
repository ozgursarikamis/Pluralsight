using Marvin.StreamExtensions;
using Movies.Client.Model;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace Movies.Client.Services
{
    public class CancellationService : IIntegrationService
    {
        private const string appJson = "application/json";
        //private static readonly HttpClient httpClient = new HttpClient();
        private static HttpClient httpClient = new HttpClient(
            new HttpClientHandler { AutomaticDecompression = DecompressionMethods.GZip });

        private CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        public CancellationService()
        {
            httpClient.BaseAddress = new Uri("http://localhost:57863/");
            httpClient.Timeout = new TimeSpan(0, 0, 5);
            httpClient.DefaultRequestHeaders.Clear();
        }
        public async Task Run()
        {
            //cancellationTokenSource.CancelAfter(2000);
            //await GetTrailerAndCancel(cancellationTokenSource.Token);
            await GetTrailerAndHandleTimeout();
        }

        private async Task GetTrailerAndCancel(CancellationToken token)
        {
            var request = new HttpRequestMessage(
                HttpMethod.Get,
                $"api/movies/d8663e5e-7494-4f81-8739-6e0de1bea7ee/trailers/{Guid.NewGuid()}");

            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(appJson));
            request.Headers.AcceptEncoding.Add(new StringWithQualityHeaderValue("gzip")); 

            try
            {
                var cancellationTokenSource = new CancellationTokenSource();
                cancellationTokenSource.CancelAfter(2000);

                using var response = await httpClient.SendAsync(request,
                    HttpCompletionOption.ResponseHeadersRead, token);

                var stream = await response.Content.ReadAsStreamAsync();
                response.EnsureSuccessStatusCode();
                var trailer = stream.ReadAndDeserializeFromJson<Trailer>();
            }
            catch (OperationCanceledException exception)
            {
                Console.WriteLine($"An operation was cancelled with message {exception.Message}.");
            }
        }

        private async Task GetTrailerAndHandleTimeout()
        {
            var request = new HttpRequestMessage(
                HttpMethod.Get,
                $"api/movies/d8663e5e-7494-4f81-8739-6e0de1bea7ee/trailers/{Guid.NewGuid()}");
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Headers.AcceptEncoding.Add(new StringWithQualityHeaderValue("gzip"));
            try
            {
                using (var response = await httpClient.SendAsync(request,
                    HttpCompletionOption.ResponseHeadersRead))
                {
                    var stream = await response.Content.ReadAsStreamAsync();

                    response.EnsureSuccessStatusCode();
                    var trailer = stream.ReadAndDeserializeFromJson<Trailer>();
                }
            }
            catch (OperationCanceledException ocException)
            {
                Console.WriteLine($"An operation was cancelled with message {ocException.Message}.");
                // additional cleanup, ...
            }
        }
    }
}
