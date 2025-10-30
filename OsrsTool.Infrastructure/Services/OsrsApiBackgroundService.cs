using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OsrsTool.Domain.Interfaces;

namespace OsrsTool.Infrastructure.Services
{
    public class OsrsApiBackgroundService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<OsrsApiBackgroundService> _logger;
        private readonly TimeSpan _interval;

        public OsrsApiBackgroundService(IServiceScopeFactory scopeFactory,
                                        IConfiguration config,
                                        ILogger<OsrsApiBackgroundService> logger)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;

            // Lees interval uit configuratie
            var intervalSection = config.GetSection("OsrsApi:IntervalMinutes");
            int minutes;
            if (!int.TryParse(intervalSection.Value, out minutes))
                minutes = 5;

            _interval = TimeSpan.FromMinutes(Math.Max(1, minutes));
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("OsrsApiBackgroundService starting with interval {Interval}.", _interval);

            // Eerste run meteen uitvoeren
            await RunOnceAsync(stoppingToken);

            using var timer = new PeriodicTimer(_interval);
            try
            {
                while (await timer.WaitForNextTickAsync(stoppingToken))
                {
                    await RunOnceAsync(stoppingToken);
                }
            }
            catch (OperationCanceledException)
            {
                // graceful shutdown
            }

            _logger.LogInformation("OsrsApiBackgroundService stopping.");
        }

        private async Task RunOnceAsync(CancellationToken ct)
        {
            try
            {
                using var scope = _scopeFactory.CreateScope();
                var apiService = scope.ServiceProvider.GetRequiredService<IOsrsApiService>();

                _logger.LogInformation("Starting fetch/store cycle at {Time}.", DateTime.UtcNow);
                await apiService.FetchAndStoreItemsAsync();
                await apiService.FetchAndStoreLatestPricesAsync();
                _logger.LogInformation("Completed fetch/store cycle at {Time}.", DateTime.UtcNow);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during Osrs API fetch/store cycle.");
            }
        }
    }
}
