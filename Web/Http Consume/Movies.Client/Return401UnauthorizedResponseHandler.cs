using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Marvin.StreamExtensions;
using Movies.Client.Model;

namespace Movies.Client
{
    public class Return401UnauthorizedResponseHandler : HttpMessageHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var response = new HttpResponseMessage(HttpStatusCode.Unauthorized);
            return Task.FromResult(response);
        }
    }

    public class TestableClassesWithApiAccess
    {
        private readonly HttpClient _httpClient;
        public TestableClassesWithApiAccess(HttpClient client)
        {
            _httpClient = client;
        }

        public async Task<Movie> GetMovie(CancellationToken cancellationToken)
        {
            var request = new HttpRequestMessage(
                HttpMethod.Get,
                "api/movies/030a43b0-f9a5-405a-811c-bf342524b2be");
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Headers.AcceptEncoding.Add(new StringWithQualityHeaderValue("gzip"));

            using var response = await _httpClient.SendAsync(request,
                HttpCompletionOption.ResponseHeadersRead,
                cancellationToken);
            if (!response.IsSuccessStatusCode)
            {
                // inspect the status code
                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    // show this to the user
                    Console.WriteLine("The requested movie cannot be found.");
                    return null;
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    // trigger a login flow
                    throw new UnauthorizedApiAccessException();
                }

                response.EnsureSuccessStatusCode();
            }
            var stream = await response.Content.ReadAsStreamAsync();
            return stream.ReadAndDeserializeFromJson<Movie>();
        }
    }
}