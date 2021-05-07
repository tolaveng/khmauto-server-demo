using Data.Api.SignalR;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Data.BackgrooundServices
{
    public class BackupService : IHostedService, IDisposable
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<BackupService> _logger;
        private readonly IHubContext<BackupHub, IBackupHub> _backupHub;

        private readonly CancellationTokenSource _stoppingCts = new CancellationTokenSource();

        public BackupService(IServiceProvider serviceProvider, ILogger<BackupService> logger,
            IHubContext<BackupHub, IBackupHub> backupHub)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
            _backupHub = backupHub;
        }

        //protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        //{
        //    _logger.LogDebug($"Start backup database");
        //    while (!cancellationToken.IsCancellationRequested)
        //    {
        //        _logger.LogDebug($"doing work...");
        //        await Task.Delay(TimeSpan.FromSeconds(1), cancellationToken);
        //    }
        //    _logger.LogDebug($"Finish backup database");
        //    await _backupHub.Clients.Group("123").JobUpdate("123", 50);
        //}

        //public override async Task StopAsync(CancellationToken cancellationToken)
        //{
        //    _logger.LogInformation("Consume Scoped Service Hosted Service is stopping.");

        //    await base.StopAsync(cancellationToken);
        //}

        private async Task BackupDatabase(CancellationToken cancellationToken)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                //IConfigurationManager conf = scope.ServiceProvider.GetRequiredService<IConfigurationManager>();
                //var scopedProcessingService =
                //scope.ServiceProvider
                //    .GetRequiredService<IScopedAlertingService>();

                //await scopedProcessingService.DoWork(stoppingToken);
                await Task.Run(() =>
                {
                });
            }
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await BackupDatabase(cancellationToken);
        }

        public void Dispose()
        {
            _stoppingCts.Cancel();
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            _stoppingCts.Cancel();
        }
    }
}
