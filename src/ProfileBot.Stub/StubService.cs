using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using WireMock.Handlers;
using WireMock.Logging;
using WireMock.Server;
using WireMock.Settings;

namespace ProfileBot.Stub
{
    internal class StubService(ILogger<StubService> logger) : BackgroundService
    {
        private WireMockServer? _server;

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            var settings = new WireMockServerSettings
            {
                Port = 6137,
                AllowPartialMapping = true,
                StartAdminInterface = true,
                ReadStaticMappings = true,
                WatchStaticMappings = true,
                WatchStaticMappingsInSubdirectories = true,
                FileSystemHandler = new LocalFileSystemHandler(),
                Logger = new WireMockConsoleLogger(),
            };

            _server = WireMockServer.Start(settings);

            logger.LogInformation("WireMock server running at {url}", string.Join(", ", _server.Urls));


            return base.StartAsync(cancellationToken);
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            return Task.CompletedTask;
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            logger.LogInformation("Stopping WireMock server...");
            _server?.Stop();
            _server?.Dispose();
            return base.StopAsync(cancellationToken);
        }
    }
}
