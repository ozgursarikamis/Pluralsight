using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Polly;
using Polly.Fallback;
using Polly.Retry;

namespace PollyBefore.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class CatalogController : Controller
    { 
        private readonly RetryPolicy<HttpResponseMessage> _httpRetryPolicy;
        private readonly FallbackPolicy<HttpResponseMessage> _httpRequestFallbackPolicy;
        private int _cachedNumber = 0;
        
        private HttpClient _httpClient;
        public CatalogController()
        {
            _httpRetryPolicy = Policy.HandleResult<HttpResponseMessage>(r => !r.IsSuccessStatusCode).RetryAsync(3);

            _httpRequestFallbackPolicy = Policy
                .HandleResult<HttpResponseMessage>(r => r.StatusCode == HttpStatusCode.InternalServerError)
                .FallbackAsync(new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new ObjectContent(_cachedNumber.GetType(), _cachedNumber, new JsonMediaTypeFormatter())
                });
        }

        private void PerformReauthorization()
        {
            _httpClient = GetHttpClient();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var httpClient = GetHttpClient();
            string requestEndpoint = $"inventory/{id}";

            var response = await _httpRequestFallbackPolicy.ExecuteAsync(
                () => _httpRetryPolicy.ExecuteAsync((() => _httpClient.GetAsync(requestEndpoint)))
            );

            if (response.IsSuccessStatusCode)
            {
                int itemsInStock = JsonConvert.DeserializeObject<int>(await response.Content.ReadAsStringAsync());
                return Ok(itemsInStock);
            }

            return StatusCode((int) response.StatusCode, response.Content.ReadAsStringAsync());
        }

        private HttpClient GetHttpClient()
        {    
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri(@"http://localhost:57697/api/");
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            
            return _httpClient;
        }
    }
}
