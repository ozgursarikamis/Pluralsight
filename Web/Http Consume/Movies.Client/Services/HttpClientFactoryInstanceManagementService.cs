using Marvin.StreamExtensions;
using Movies.Client.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace Movies.Client.Services
{
    public class HttpClientFactoryInstanceManagementService : IIntegrationService
    {
        private readonly CancellationTokenSource _cancellationTokenSource =
            new CancellationTokenSource();

        private const string appJson = "application/json";
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly MoviesClient _moviesClient;

        public HttpClientFactoryInstanceManagementService(IHttpClientFactory httpClientFactory, MoviesClient moviesClient)
        {
            _httpClientFactory = httpClientFactory;
            _moviesClient = moviesClient;
        }
        public async Task Run()
        {
            //await TestDisposeHttpClient(_cancellationTokenSource.Token);
            //await TestReuseHttpClient(_cancellationTokenSource.Token);
            //await GetMoviesWithHttpClientFromFactory(_cancellationTokenSource.Token);
            //await GetMoviesWithNamedHttpClientFromFactory(_cancellationTokenSource.Token);
            await GetMoviesWithTypedHttpClientFromFactory(_cancellationTokenSource.Token);
        }
        private async Task GetMoviesWithTypedHttpClientFromFactory(
            CancellationToken cancellationToken)
        {
            var request = new HttpRequestMessage(
                HttpMethod.Get,
                "api/movies");
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Headers.AcceptEncoding.Add(new StringWithQualityHeaderValue("gzip"));
            await GetMoviesViaMoviesClient(cancellationToken);
        }
        private async Task GetMoviesViaMoviesClient(CancellationToken cancellationToken)
        {
            var movies = await _moviesClient.GetMovies(cancellationToken);
        }

        private async Task GetMoviesWithNamedHttpClientFromFactory(
            CancellationToken cancellationToken)
        {
            var httpClient = _httpClientFactory.CreateClient("MoviesClient");
            var request = new HttpRequestMessage(
                HttpMethod.Get,
                "api/movies");
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Headers.AcceptEncoding.Add(new StringWithQualityHeaderValue("gzip"));
            try
            {
                using (var response = await httpClient.SendAsync(request,
                    HttpCompletionOption.ResponseHeadersRead,
                    cancellationToken))
                {
                    var stream = await response.Content.ReadAsStreamAsync();
                    response.EnsureSuccessStatusCode();
                    var movies = stream.ReadAndDeserializeFromJson<List<Movie>>();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        private async Task GetMoviesWithHttpClientFromFactory(CancellationToken cancellationToken)
        {
            var httpClient = _httpClientFactory.CreateClient();
            var request = new HttpRequestMessage(
                HttpMethod.Get, "http://localhost:57863/api/movies"
                );
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(appJson));
            try
            { 
                using var response = await httpClient.SendAsync(request,
                    HttpCompletionOption.ResponseHeadersRead, cancellationToken
                    );

                var stream = await response.Content.ReadAsStreamAsync();
                response.EnsureSuccessStatusCode();
                var movies = stream.ReadAndDeserializeFromJson<List<Movie>>();
                Debug.WriteLine(movies.Count);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        private async Task TestReuseHttpClient(CancellationToken cancellationToken)
        {
            var httpClient = new HttpClient();

            for (int i = 0; i < 10; i++)
            {
                var request = new HttpRequestMessage(
                HttpMethod.Get,
                "https://www.google.com");

                using (var response = await httpClient.SendAsync(request,
                    HttpCompletionOption.ResponseHeadersRead,
                    cancellationToken))
                {
                    var stream = await response.Content.ReadAsStreamAsync();
                    response.EnsureSuccessStatusCode();

                    Console.WriteLine($"Request completed with status code {response.StatusCode}");
                }
            }
        }
        private async Task TestDisposeHttpClient(CancellationToken cancellationToken)
        {
            for (var i = 0; i < 10; i++)
            {
                using (var httpClient = new HttpClient())
                {
                    var request = new HttpRequestMessage(
                        HttpMethod.Get,
                        "https://www.google.com");

                    using (var response = await httpClient.SendAsync(request,
                        HttpCompletionOption.ResponseHeadersRead,
                        cancellationToken))
                    {
                        var stream = await response.Content.ReadAsStreamAsync();
                        response.EnsureSuccessStatusCode();

                        Console.WriteLine($"Request completed with status code" +
                            $" {response.StatusCode}");
                    }
                }
            }
        }
    }
}
