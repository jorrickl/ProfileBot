using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ProfileBot.DevelopmentHost;

await Host.CreateDefaultBuilder(args)
          .ConfigureServices((context, services)
            => services.AddHostedService<StubService>())
          .Build()
          .RunAsync().ConfigureAwait(false);