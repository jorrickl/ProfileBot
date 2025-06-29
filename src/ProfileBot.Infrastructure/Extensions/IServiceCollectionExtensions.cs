using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using ProfileBot.Infrastructure.Adapters;
using ProfileBot.Infrastructure.Clients;
using ProfileBot.Infrastructure.Configuration;
using ProfileBot.Infrastructure.Interfaces;

namespace ProfileBot.Infrastructure.Extensions
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            ConfigureOptions(services, configuration);
            return services.ConfigureProfileServices();
        }

        private static IServiceCollection ConfigureProfileServices(this IServiceCollection services)
        {
            services.AddTransient<IProfileAdapter, ProfileAdapter>();
            services.AddHttpClient<IProfileClient, ProfileClient>((sp, client) =>
            {
                var options = sp.GetRequiredService<IOptions<RuneScapeOptions>>().Value;
                client.BaseAddress = new Uri(options.ProfileBaseUrl);
                client.DefaultRequestHeaders.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.7");
            });
            return services;
        }

        private static void ConfigureOptions(IServiceCollection services, IConfiguration configuration)
        {
            services.AddOptions<RuneScapeOptions>()
                    .Bind(configuration.GetRequiredSection(RuneScapeOptions.ConfigurationSectionName))
                    .ValidateDataAnnotations()
                    .ValidateOnStart();
        }
    }
}
