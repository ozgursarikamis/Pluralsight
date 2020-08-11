using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TennisBookings.Web.Configuration;

namespace TennisBookings.Web
{
    public class ValidateOptionsService : IHostedService
    {
        private readonly ILogger<ValidateOptionsService> _logger;
        private readonly IHostApplicationLifetime _appLifeTime;
        private readonly IOptions<HomePageConfiguration> _homePageConfig;
        private readonly IOptionsMonitor<ExternalServicesConfig> _externalServiceConfig;

        public ValidateOptionsService(ILogger<ValidateOptionsService> logger, IHostApplicationLifetime appLifeTime, IOptions<HomePageConfiguration> homePageConfig, IOptionsMonitor<ExternalServicesConfig> externalServiceConfig)
        {
            _logger = logger;
            _appLifeTime = appLifeTime;
            _homePageConfig = homePageConfig;
            _externalServiceConfig = externalServiceConfig;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            try
            {
                _ = _homePageConfig.Value;
                _ = _externalServiceConfig.Get(ExternalServicesConfig.ProductsApi);
                _ = _externalServiceConfig.Get(ExternalServicesConfig.WeatherApi);
            }
            catch (OptionsValidationException exception)
            {
                _logger.LogError("One or more options validation checks failed");
                foreach (var failure in exception.Failures)
                {
                    _logger.LogError(failure);
                }

                _appLifeTime.StopApplication();
            }
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
