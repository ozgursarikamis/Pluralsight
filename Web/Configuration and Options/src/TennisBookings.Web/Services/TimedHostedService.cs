using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace TennisBookings.Web.Services
{
    public class TimedHostedService : IHostedService, IDisposable
    {
        private int _executionCount;
        private readonly ILogger<TimedHostedService> _logger;
        private Timer _timer;

        public TimedHostedService(ILogger<TimedHostedService> logger)
        {
            _logger = logger;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            const string timerLog = "Timed Hosted Service is running";
            _logger.LogInformation(timerLog);
            Debug.WriteLine(timerLog);

            _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromSeconds(5));

            return Task.CompletedTask;
        }

        private void DoWork(object state)
        {
            var count = Interlocked.Increment(ref _executionCount);
            var message = $"THS Count: {count}";
            _logger.LogInformation(message);
            Debug.WriteLine(message);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            const string message = "Timed Hosted Service is stopping.";
            Debug.WriteLine(message);

            _logger.LogInformation(message);
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
