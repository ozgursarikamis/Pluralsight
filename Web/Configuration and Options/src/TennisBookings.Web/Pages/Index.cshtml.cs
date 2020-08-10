using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using TennisBookings.Web.Services;

namespace TennisBookings.Web.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IWeatherForecaster _weatherForecaster;
        private readonly IGreetingService _greetingService;
        private readonly IConfiguration _configuration;

        public IndexModel(IGreetingService greetingService, IConfiguration configuration, IWeatherForecaster weatherForecaster)
        {
            _greetingService = greetingService;
            _configuration = configuration;
            _weatherForecaster = weatherForecaster;
        }

        public string Greeting { get; private set; }
        public bool ShowGreeting => !string.IsNullOrEmpty(Greeting);
        public string ForecastSectionTitle { get; private set; }
        public string WeatherDescription { get; private set; }
        public bool ShowWeatherForecast { get; private set; }

        public async Task OnGet()
        {
            //var homePageFeatures = _configuration.GetSection("Features:HomePage");
            var feature = new Features();
            _configuration.Bind("Features:HomePage", feature);

            if (feature.EnableWeatherForecast)
            {
                Greeting = _greetingService.GetRandomGreeting();
            }

            //ShowWeatherForecast = homePageFeatures.GetValue<bool>("EnableWeatherForecast");
            ShowWeatherForecast = feature.EnableWeatherForecast && _weatherForecaster.ForecastEnabled;

            if (ShowWeatherForecast)
            {
                //var title = homePageFeatures["ForecastSectionTitle"];
                var title = feature.ForecastSectionTitle;
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
        }

        private class Features
        {
            public bool EnableGreeting { get; set; }
            public bool EnableWeatherForecast { get; set; }
            public string ForecastSectionTitle { get; set; }
        }
    }
}
