using Microsoft.Extensions.Options;

namespace TennisBookings.Web.Configuration
{
    public class HomePageConfigurationValidation : IValidateOptions<HomePageConfiguration>
    {
        private readonly IProfanityChecker _profanityChecker;
        private readonly bool _checkerProfanity;
        private readonly WeatherForecastingConfiguration _weatherConfig;

        public HomePageConfigurationValidation(
            IContentConfiguration contentConfig,
            IOptions<WeatherForecastingConfiguration> weatherConfig,
            IProfanityChecker profanityChecker)
        {
            _profanityChecker = profanityChecker;
            _weatherConfig = weatherConfig.Value;
            _checkerProfanity = contentConfig.CheckForProfanity;
        }

        public ValidateOptionsResult Validate(string name, HomePageConfiguration options)
        {
            if (_weatherConfig.EnableWeatherForecast && options.EnableWeatherForecast
            && string.IsNullOrEmpty(options.ForecastSectionTitle)
            )
            {
                return ValidateOptionsResult.Fail("Title is required");
            }

            if (_checkerProfanity && _profanityChecker.ContainsProfanity(options.ForecastSectionTitle))
            {
                return ValidateOptionsResult.Fail("title contains a blocked profanity word");
            }
            return ValidateOptionsResult.Success;
        }
    }
}
