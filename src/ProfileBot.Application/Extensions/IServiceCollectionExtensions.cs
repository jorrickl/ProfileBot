using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using ProfileBot.Application.Interfaces;
using ProfileBot.Application.Profiles.Formatting;
using ProfileBot.Application.Profiles.Get;
using ProfileBot.SharedKernel.Behaviors;

namespace ProfileBot.Application.Extensions
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddValidatorsFromAssemblyContaining<GetProfileQueryValidator>(ServiceLifetime.Transient);

            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssemblyContaining<GetProfileQueryHandler>();
                cfg.RegisterServicesFromAssemblyContaining(typeof(ResultValidatingPipeline<,>));
                cfg.AddOpenBehavior(typeof(ResultValidatingPipeline<,>));
            });

            services.AddTransient<IActivityFormatter, ActivityFormatter>();
            services.AddTransient<IActivityMatcherFactory, ActivityMatcherFactory>();

            // Register ActivityMatchers using the factory
            services.AddSingleton<IEnumerable<IActivityMatcher>>(provider =>
            {
                var factory = provider.GetRequiredService<IActivityMatcherFactory>();
                return
                [
                    factory.Create<LevelUpActivityMatcher>(@"I levelled my (.*) skill, I am now level (\d.*)\.", "**{0}** just leveled **{1}** to {2}"),
                    factory.Create<XpMilestoneActivityMatcher>(@"(\d+)XP in ([A-Za-z]+)", "**{0}** got **{1}m** XP in **{2}** :tada:")
                ];
            });

            return services;
        }
    }
}
