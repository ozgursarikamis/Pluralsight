using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using TennisBookings.Web.Configuration;
using TennisBookings.Web.External;
using TennisBookings.Web.Services;

namespace TennisBookings.Web.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IWeatherForecaster _weatherForecaster;
        private readonly IGreetingService _greetingService;
        //private readonly IConfiguration _configuration;
        private readonly IProductsApiClient _productsApiClient;
        private readonly HomePageConfiguration _homePageConfiguration;

        public IndexModel(
            IGreetingService greetingService,
            IWeatherForecaster weatherForecaster,
            IOptionsSnapshot<HomePageConfiguration> options, 
            IProductsApiClient productsApiClient)
        {
            _greetingService = greetingService;
            _homePageConfiguration = options.Value;
            _weatherForecaster = weatherForecaster;
            _productsApiClient = productsApiClient;

            GreetingColor = _greetingService.GreetingColour ?? "black";
        }

        public string GreetingColor { get; set; }

        public string Greeting { get; private set; }
        public bool ShowGreeting => !string.IsNullOrEmpty(Greeting);
        public string ForecastSectionTitle { get; private set; }
        public string WeatherDescription { get; private set; }
        public bool ShowWeatherForecast { get; private set; }

        public async Task OnGet()
        {
            if (_homePageConfiguration.EnableWeatherForecast)
            {
                Greeting = _greetingService.GetRandomGreeting();
            }

            //ShowWeatherForecast = homePageFeatures.GetValue<bool>("EnableWeatherForecast");
            ShowWeatherForecast = _homePageConfiguration.EnableWeatherForecast && _weatherForecaster.ForecastEnabled;

            if (ShowWeatherForecast)
            {
                //var title = homePageFeatures["ForecastSectionTitle"];
                var title = _homePageConfiguration.ForecastSectionTitle;
                ForecastSectionTitle = string.IsNullOrEmpty(title) ? "How is the weather" : title;

                var currentWeather = await _weatherForecaster.GetCurrentWeatherAsync();
                if (currentWeather != null)
                {
                    switch (currentWeather.Description)
                    {
                        case "Sun":
                            WeatherDescription = "sunny";
                            break;
                        case "Cloud":
                            WeatherDescription = "cloudy";
                            break;
                        case "Rain":
                            WeatherDescription = "rainy";
                            break;
                        case "Snow":
                            WeatherDescription = "Snowy";
                            break;
                        default:
                            WeatherDescription = "";
                            break;
                    }
                }
            }
            var productsResult = await _productsApiClient.GetProducts();

            Products = productsResult.Products;
        }

        public IReadOnlyCollection<Product> Products { get; set; }
    }
}
