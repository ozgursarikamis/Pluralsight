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
            var homePageFeatures = _configuration.GetSection("Features:HomePage");
            if (homePageFeatures.GetValue<bool>("EnableGreeting"))
            {
                Greeting = _greetingService.GetRandomGreeting();
            }

            ShowWeatherForecast = homePageFeatures.GetValue<bool>("EnableWeatherForecast");
            if (ShowWeatherForecast)
            {
                var title = homePageFeatures["ForecastSectionTitle"];
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
    }
}
