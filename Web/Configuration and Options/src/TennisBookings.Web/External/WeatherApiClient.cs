using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using TennisBookings.Web.External.Models;
using Microsoft.Extensions.Options;
using TennisBookings.Web.Configuration;

namespace TennisBookings.Web.External
{
    public class WeatherApiClient : IWeatherApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<WeatherApiClient> _logger;

        public WeatherApiClient(
            HttpClient httpClient, 
            IOptionsMonitor<ExternalServiceConfiguration> config, 
            ILogger<WeatherApiClient> logger)
        {
            var weatherApiOptions = config.Get(ExternalServiceConfiguration.WeatherApi);
            var externalServiceConfig = weatherApiOptions;

            httpClient.BaseAddress = new Uri(externalServiceConfig.Url);

            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<WeatherApiResult> GetWeatherForecastAsync()
        {
            const string path = "api/currentWeather/Brighton";

            try
            {
                var response = await _httpClient.GetAsync(path);

                if (!response.IsSuccessStatusCode)
                {
                    return null;
                }

                var content = await response.Content.ReadAsAsync<WeatherApiResult>();

                return content;
            }
            catch (HttpRequestException e)
            {
                _logger.LogError(e, "Failed to get weather data from API");
            }

            return null;
        }
    }
    public class ProductsApiClient : IProductsApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<ProductsApiClient> _logger;
        private readonly IMemoryCache _cache;
        private readonly IOptionsMonitor<ExternalServiceConfiguration> _productsApiConfig;

        public ProductsApiClient(HttpClient httpClient, IOptionsMonitor<ExternalServiceConfiguration> options, ILogger<ProductsApiClient> logger, IMemoryCache cache)
        {
            var externalServicesConfig = options.Get(ExternalServiceConfiguration.ProductsApi);

            httpClient.BaseAddress = new Uri(externalServicesConfig.Url);

            _httpClient = httpClient;
            _logger = logger;
            _cache = cache;
            _productsApiConfig = options;
        }

        public async Task<ProductsApiResult> GetProducts()
        {
            const string cacheKey = "Products";

            if (_cache.TryGetValue<ProductsApiResult>(cacheKey, out var forecast))
            {
                return forecast;
            }

            const string path = "api/products";

            try
            {
                var response = await _httpClient.GetAsync(path);

                if (!response.IsSuccessStatusCode)
                {
                    return null;
                }

                var content = await response.Content.ReadAsAsync<ProductsApiResult>();

                if (content != null)
                {
                    var cacheOptions = new MemoryCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(_productsApiConfig.Get(ExternalServiceConfiguration.ProductsApi).MinsToCache)
                    };

                    _cache.Set(cacheKey, content, cacheOptions);
                }

                return content;
            }
            catch (HttpRequestException e)
            {
                _logger.LogError(e, "Failed to get products data from API");
            }

            return null;
        }
    }
}
