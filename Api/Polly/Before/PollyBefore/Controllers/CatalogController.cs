using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Polly;
using Polly.Retry;

namespace PollyBefore.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class CatalogController : Controller
    { 
        private readonly RetryPolicy<HttpResponseMessage> _httpResponsePolicy;

        public CatalogController()
        {
            _httpResponsePolicy = Policy
                .HandleResult<HttpResponseMessage>(r => !r.IsSuccessStatusCode)
                .RetryAsync(3, (httpResponseMessage, retryCount) =>
                {
                    if (httpResponseMessage.Result.StatusCode == HttpStatusCode.NotFound)
                    {
                        
                    } else if (httpResponseMessage.Result.StatusCode == HttpStatusCode.Conflict)
                    {
                        
                    }
                });
        }
        
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var httpClient = GetHttpClient();
            string requestEndpoint = $"inventory/{id}";

            // var response = await httpClient.GetAsync(requestEndpoint);
            var response = await _httpResponsePolicy.ExecuteAsync(() => httpClient.GetAsync(requestEndpoint));

            if (!response.IsSuccessStatusCode)
                return StatusCode((int) response.StatusCode, response.Content.ReadAsStringAsync());
            var itemsInStock = JsonConvert.DeserializeObject<int>(await response.Content.ReadAsStringAsync());
            return Ok(itemsInStock);

        }

        private HttpClient GetHttpClient()
        {
            var httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(@"http://localhost:57697/api/");
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            return httpClient;
        }
    }
}
