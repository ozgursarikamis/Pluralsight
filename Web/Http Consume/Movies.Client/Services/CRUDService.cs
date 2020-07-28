using Movies.Client.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Movies.Client.Services
{
    public class CRUDService : IIntegrationService
    {
        private const string appJson = "application/json";
        private static readonly HttpClient httpClient = new HttpClient();
        public CRUDService()
        {
            httpClient.BaseAddress = new Uri("http://localhost:57863/");
            httpClient.Timeout = new TimeSpan(0, 0, 30);

            //// The API responses as XML by default:
            //httpClient.DefaultRequestHeaders.Accept.Add(
            //    new MediaTypeWithQualityHeaderValue(appJson)
            //    );
            //httpClient.DefaultRequestHeaders.Accept.Add(
            //    new MediaTypeWithQualityHeaderValue("application/xml", 0.9)
            //    );
        }
        public async Task Run()
        {
            // await GetResource();
            // await GetResourceThroughHttpRequestMessage();
            //await CreateResource();
            await UpdateResource();
        }

        public async Task GetResource()
        {
            var response = await httpClient.GetAsync("api/movies");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();

            var movies = new List<Movie>();
            if (response.Content.Headers.ContentType.MediaType == appJson)
            {
                movies = JsonConvert.DeserializeObject<List<Movie>>(content);
            }
            else if (response.Content.Headers.ContentType.MediaType == "application/xml")
            {
                var serializer = new XmlSerializer(typeof(List<Movie>));
                movies = (List<Movie>)serializer.Deserialize(new StringReader(content));
            }
        }
        
        public async Task GetResourceThroughHttpRequestMessage()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "api/movies");
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(appJson));

            var response = await httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var movies = JsonConvert.DeserializeObject<List<Movie>>(content);
        }

        public async Task CreateResource()
        {
            var movieToCreate = new MovieForCreation
            {
                Title = "Reservoir Dogs",
                Description = "Some desc",
                DirectorId = Guid.Parse("69cb27e5-46ac-4434-a462-cf2826cd61aa"),
                ReleaseDate = new DateTimeOffset(new DateTime(1992, 9, 2)),
                Genre = "Crime, Drama"
            };
            var serializedMovie = JsonConvert.SerializeObject(movieToCreate);

            var request = new HttpRequestMessage(HttpMethod.Post, "api/movies");
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(appJson));

            request.Content = new StringContent(serializedMovie);
            request.Content.Headers.ContentType = new MediaTypeHeaderValue(appJson);

            var response = await httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var createdMovie = JsonConvert.DeserializeObject<Movie>(content);
        }

        public async Task UpdateResource()
        {
            var movieToUpdate = new MovieForUpdate
            {
                Title = "Pulp Fiction",
                Description = "Some desc",
                DirectorId = Guid.Parse("69cb27e5-46ac-4434-a462-cf2826cd61aa"),
                ReleaseDate = new DateTimeOffset(new DateTime(1992, 9, 2)),
                Genre = "Crime, Drama"
            };
            var serializedMovieToUpdate = JsonConvert.SerializeObject(movieToUpdate);
            const string url = "api/movies/5b1c2b4d-48c7-80c3-cc796ad49c6b";
            
            var request = new HttpRequestMessage(HttpMethod.Put, url);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(appJson));
            request.Content = new StringContent(serializedMovieToUpdate);
            request.Content.Headers.ContentType = new MediaTypeHeaderValue(appJson);

            var response = await httpClient.SendAsync(request);
            var content = await response.Content.ReadAsStringAsync();
            var updatedMovie = JsonConvert.DeserializeObject<Movie>(content);
            Debug.WriteLine(updatedMovie);
        }
    }
}
