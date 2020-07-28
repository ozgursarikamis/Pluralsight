using Microsoft.AspNetCore.JsonPatch;
using Movies.Client.Model;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Movies.Client.Services
{
    public class PartialUpdateService : IIntegrationService
    {
        private const string appJson = "application/json";
        private static readonly HttpClient httpClient = new HttpClient();
        public PartialUpdateService()
        {
            httpClient.BaseAddress = new Uri("http://localhost:57863/");
            httpClient.Timeout = new TimeSpan(0, 0, 30);
            httpClient.DefaultRequestHeaders.Clear();
        }
        public async Task Run()
        {
            await PatchResource();
        }
        
        public async Task PatchResource()
        {
            var patchDoc = new JsonPatchDocument<MovieForUpdate>();
            patchDoc.Replace(m => m.Title, "Update Title");
            patchDoc.Remove(m => m.Description);

            //var serializedChangeSet = JsonConvert.SerializeObject(patchDoc);
            //var request = new HttpRequestMessage(HttpMethod.Patch,
            //    "api/movies/bb6a100a-053f-4bf8-b271-60ce3aae6eb5");

            //request.Content = new StringContent(serializedChangeSet);
            //request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json-patch+json"));

            //var response = await httpClient.SendAsync(request);
            //response.EnsureSuccessStatusCode();

            //var content = await response.Content.ReadAsStringAsync();
            //var updatedMovie = JsonConvert.DeserializeObject<Movie>(content);
            //Debug.WriteLine(updatedMovie);

            var response = await httpClient.PatchAsync(
                "api/movies/bb6a100a-053f-4bf8-b271-60ce3aae6eb5",
                new StringContent(
                    JsonConvert.SerializeObject(patchDoc),
                    Encoding.UTF8, "application/json-patch+json"));

            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var updatedMovie = JsonConvert.DeserializeObject<Movie>(content);
            Debug.WriteLine(updatedMovie);
        }
    }
}
