using Marvin.StreamExtensions;
using Movies.Client.Model;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Movies.Client.Services
{
    public class StreamService : IIntegrationService
    {
        private const string appJson = "application/json";
        //private static readonly HttpClient httpClient = new HttpClient();
        private static HttpClient httpClient = new HttpClient(
            new HttpClientHandler { AutomaticDecompression = DecompressionMethods.GZip });

        public StreamService()
        {
            httpClient.BaseAddress = new Uri("http://localhost:57863/");
            httpClient.Timeout = new TimeSpan(0, 0, 30);
            httpClient.DefaultRequestHeaders.Clear();
        }
        public async Task Run()
        {
            // await GetPosterWithStream();
            // await GetPosterWithStreamAndCompletionMode();
            //await PostAndReadPosterWithStreams();
            //await TestPostPosterWithoutStream();
            //await TestPostPosterWithStream();
            //await TestPostAndReadPosterWithStreams();
            await GetPosterWithGZipCompression();
        }

        private async Task GetPosterWithGZipCompression()
        {
            var request = new HttpRequestMessage(
                HttpMethod.Get,
                $"api/movies/d8663e5e-7494-4f81-8739-6e0de1bea7ee/posters/{Guid.NewGuid()}");
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Headers.AcceptEncoding.Add(new StringWithQualityHeaderValue("gzip"));

            using (var response = await httpClient.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();

                var stream = await response.Content.ReadAsStreamAsync();
                var poster = stream.ReadAndDeserializeFromJson<Poster>();
            }
        }
        private async Task PostPosterWithoutStream()
        {
            // generate a movie poster of 500KB
            var random = new Random();
            var generatedBytes = new byte[524288];
            random.NextBytes(generatedBytes);

            var posterForCreation = new PosterForCreation()
            {
                Name = "A new poster for The Big Lebowski",
                Bytes = generatedBytes
            };

            var serializedPosterForCreation = JsonConvert.SerializeObject(posterForCreation);
            var request = new HttpRequestMessage(HttpMethod.Post, "api/movies/d8663e5e-7494-4f81-8739-6e0de1bea7ee/posters");

            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(appJson));
            request.Content = new StringContent(serializedPosterForCreation);
            request.Content.Headers.ContentType = new MediaTypeHeaderValue(appJson);

            var response = await httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var createdMovie = JsonConvert.DeserializeObject<Poster>(content);
        }

        private async Task TestPostPosterWithoutStream()
        {
            await PostAndReadPosterWithStreams();

            var stopWatch = Stopwatch.StartNew();

            for (int i = 0; i < 200; i++)
            {
                await PostAndReadPosterWithStreams();
            }

            stopWatch.Stop();
            Console.WriteLine($"Elapsed milliseconds without stream: " +
            $"{stopWatch.ElapsedMilliseconds}, " +
            $"averaging {stopWatch.ElapsedMilliseconds / 200} milliseconds/request");
        }
        private async Task TestPostPosterWithStream()
        {
            // warmup
            await PostPosterWithStream();

            // start stopwatch 
            var stopWatch = Stopwatch.StartNew();

            // run requests
            for (int i = 0; i < 200; i++)
            {
                await PostPosterWithStream();
            }

            // stop stopwatch
            stopWatch.Stop();
            Console.WriteLine($"Elapsed milliseconds with stream: " +
                $"{stopWatch.ElapsedMilliseconds}, " +
                $"averaging {stopWatch.ElapsedMilliseconds / 200} milliseconds/request");
        }

        private async Task TestPostAndReadPosterWithStreams()
        {
            // warmup
            await PostAndReadPosterWithStreams();

            // start stopwatch 
            var stopWatch = Stopwatch.StartNew();

            // run requests
            for (int i = 0; i < 200; i++)
            {
                await PostAndReadPosterWithStreams();
            }

            // stop stopwatch
            stopWatch.Stop();
            Console.WriteLine($"Elapsed milliseconds with stream on post and read: " +
                $"{stopWatch.ElapsedMilliseconds}, " +
                $"averaging {stopWatch.ElapsedMilliseconds / 200} milliseconds/request");
        }

        private async Task PostAndReadPosterWithStreams()
        {
            // generate a movie poster of 500KB
            var random = new Random();
            var generatedBytes = new byte[524288];
            random.NextBytes(generatedBytes);

            var posterForCreation = new PosterForCreation()
            {
                Name = "A new poster for The Big Lebowski",
                Bytes = generatedBytes
            };

            var memoryContentStream = new MemoryStream();
            memoryContentStream.SerializeToJsonAndWrite(posterForCreation,
                          new UTF8Encoding(), 1024, true);

            memoryContentStream.Seek(0, SeekOrigin.Begin);
            using (var request = new HttpRequestMessage(
              HttpMethod.Post,
              $"api/movies/d8663e5e-7494-4f81-8739-6e0de1bea7ee/posters"))
            {
                request.Headers.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));

                using (var streamContent = new StreamContent(memoryContentStream))
                {
                    request.Content = streamContent;
                    request.Content.Headers.ContentType =
                      new MediaTypeHeaderValue("application/json");

                    using (var response = await httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead))
                    {
                        response.EnsureSuccessStatusCode();
                        var stream = await response.Content.ReadAsStreamAsync();
                        var poster = stream.ReadAndDeserializeFromJson<Poster>();
                    }
                }
            }
        }
        private async Task PostPosterWithStream()
        {
            // generate a movie poster of 500KB
            var random = new Random();
            var generatedBytes = new byte[524288];
            random.NextBytes(generatedBytes);

            var posterForCreation = new PosterForCreation()
            {
                Name = "A new poster for The Big Lebowski",
                Bytes = generatedBytes
            };

            var memoryContentStream = new MemoryStream();
            memoryContentStream.SerializeToJsonAndWrite(posterForCreation,
                new UTF8Encoding(), 1024, true);

            //using (var streamWriter = new StreamWriter(memoryContentStream, 
            //    new UTF8Encoding(), 1024, true))
            //{
            //    using (var jsonTextWriter = new JsonTextWriter(streamWriter))
            //    {
            //        var jsonSerializer = new JsonSerializer();
            //        jsonSerializer.Serialize(jsonTextWriter, posterForCreation);
            //        jsonTextWriter.Flush();
            //    }
            //}

            memoryContentStream.Seek(0, SeekOrigin.Begin);
            using (var request = new HttpRequestMessage(
              HttpMethod.Post,
              $"api/movies/d8663e5e-7494-4f81-8739-6e0de1bea7ee/posters"))
            {
                request.Headers.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));

                using (var streamContent = new StreamContent(memoryContentStream))
                {
                    request.Content = streamContent;
                    request.Content.Headers.ContentType =
                      new MediaTypeHeaderValue("application/json");

                    var response = await httpClient.SendAsync(request);
                    response.EnsureSuccessStatusCode();

                    var createdContent = await response.Content.ReadAsStringAsync();
                    var createdPoster = JsonConvert.DeserializeObject<Poster>(createdContent);
                }
            }
        }
        private async Task GetPosterWithStream()
        {
            const string posterId = "bb6a100a-053f-4bf8-b271-60ce3aae6eb5";
            var request = new HttpRequestMessage(
                HttpMethod.Get,
                $"api/movies/{posterId}/posters/{Guid.NewGuid()}");
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(appJson));

            using var response = await httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var stream = await response.Content.ReadAsStreamAsync();
            var poster = stream.ReadAndDeserializeFromJson<Poster>();

            Debug.WriteLine(poster.Name);
        }

        private async Task GetPosterWithStreamAndCompletionMode()
        {
            const string posterId = "bb6a100a-053f-4bf8-b271-60ce3aae6eb5";
            var request = new HttpRequestMessage(
                HttpMethod.Get,
                $"api/movies/{posterId}/posters/{Guid.NewGuid()}");
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(appJson));

            using var response = await httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);
            var stream = await response.Content.ReadAsStreamAsync();
            var poster = stream.ReadAndDeserializeFromJson<Poster>();
            Debug.WriteLine(poster.Name);
        }
    }
}