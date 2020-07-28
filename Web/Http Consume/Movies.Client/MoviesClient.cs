using Marvin.StreamExtensions;
using Movies.Client.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace Movies.Client
{
    public class MoviesClient
    {
        private readonly HttpClient client;
        //public HttpClient Client { get; }
        public MoviesClient(HttpClient _client)
        {
            client = _client;
            client.BaseAddress = new Uri("http://localhost:57863");
            client.Timeout = new TimeSpan(0, 0, 30);
            client.DefaultRequestHeaders.Clear();
        }

        public async Task<IEnumerable<Movie>> GetMovies(CancellationToken token)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "api/movies");

            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Headers.AcceptEncoding.Add(new StringWithQualityHeaderValue("gzip"));

            using var response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, token);
            response.EnsureSuccessStatusCode();

            var stream = await response.Content.ReadAsStreamAsync();
            return stream.ReadAndDeserializeFromJson<List<Movie>>(); 
        }
    }
}
