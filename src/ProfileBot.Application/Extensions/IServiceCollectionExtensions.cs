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
                    factory.Create<XpMilestoneActivityMatcher>(@"(\d+)XP in ([A-Za-z]+)", "**{0}** got **{1}m** XP in **{2}** :tada:"),
                    factory.Create<SingleTextActivityMatcher>(@"(\d+) Total levels gained", "**{0}** has reached a total level of {1}"),
                    factory.Create<QuestCompleteActivityMatcher>(@"Quest complete: (.*)", "**{0}** completed the quest **{1}**"),
                    factory.Create<SkillingPetActivityMatcher>(@"I found (.*), the (.*) pet.", "**{0}** found {1}, the {2} pet :tada:"),
                    factory.Create<CapeActivityMatcher>(@"Bought my first (.*) cape.", "**{0}** bought their first {1} cape :tada:"),
                    factory.Create<CapeActivityMatcher>(@"Upgraded my (.*) cape.", "**{0}** upgraded their {1} cape :tada:"),
                    factory.Create<SingleTextActivityMatcher>(@"Unlocked (Effy the effigy pet).*", "**{0}** received {1} :tada:"),
                    factory.Create<SingleTextActivityMatcher>(@"I adopted (TzRek-Jad).*", "**{0}** adopted {1} :tada:"),
                    factory.Create<SingleTextActivityMatcher>(@"I found an (Edimmu).*", "**{0}** received the {1} pet :tada:"),
                    factory.Create<SingleTextActivityMatcher>(@"I found (an? .*)", "**{0}** found {1}"),
                    factory.Create<SingleTextActivityMatcher>(@"Levelled all skills over (\d+).*", "**{0}** has achieved level {1} in all skills"),
                ];
            });

            return services;
        }
    }
}
