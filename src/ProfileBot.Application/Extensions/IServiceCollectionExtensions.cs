using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using ProfileBot.Application.Activities.Formatters;
using ProfileBot.Application.Activities.Get;
using ProfileBot.Application.Interfaces;
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

            return services;
        }
    }
}
