using System.ComponentModel.DataAnnotations;

namespace TennisBookings.Web.Configuration
{
    public class HomePageConfiguration
    {
        public bool EnableGreeting { get; set; }
        public bool EnableWeatherForecast { get; set; }
        
        [Required]
        public string ForecastSectionTitle { get; set; }
    } 

    public class WeatherForecastingConfiguration
    {
        public bool EnableWeatherForecast { get; set; }
    }


    public class GreetingConfiguration
    { 
        public string GreetingColour { get; set; }
    }

    public class ExternalServicesConfig
    {
        public const string WeatherApi = "WeatherApi";
        public const string ProductsApi = "ProductsApi";

        public string Url { get; set; }
        public int MinsToCache { get; set; }
        public string ApiKey { get; set; }
    }
}
