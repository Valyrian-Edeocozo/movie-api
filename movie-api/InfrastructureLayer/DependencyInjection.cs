using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using movie_api.ApiLayer.EndpointsCommon;
using movie_api.ApiLayer.Infrastructure;
using movie_api.ApiLayer.Models;
using movie_api.ApplicationLayer.Common.Behaviour;
using movie_api.ApplicationLayer.UtilityService.Implementation;
using movie_api.ApplicationLayer.UtilityService.Interface;
using movie_api.InfrastructureLayer.Interfaces;
using movie_api.InfrastructureLayer.Repository;
using System.Data;
using System.Reflection;
using ZymLabs.NSwag.FluentValidation;
using FluentValidationRule = ZymLabs.NSwag.FluentValidation.FluentValidationRule;

namespace movie_api.InfrastructureLayer
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
        {
            services.AddEndpointDefinitions(typeof(GapUser));

            services.AddScoped<IGenericService, GenericService>();
            services.AddScoped(typeof(ICacheRepository<>), typeof(CacheRepository<>));
            services.AddHttpClient();
            services.AddMemoryCache();

            services.AddExceptionHandler<CustomExceptionHandler>();

            services.AddScoped(provider =>
            {
                var validationRules = provider.GetService<IEnumerable<FluentValidationRule>>();
                var loggerFactory = provider.GetService<ILoggerFactory>();

                return new FluentValidationSchemaProcessor(provider, validationRules, loggerFactory);
            });
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            // Customise default API behaviour
            services.Configure<ApiBehaviorOptions>(options =>
                options.SuppressModelStateInvalidFilter = true);

            services.AddAuthorization(options => options.AddPolicy("CanPurge", policy => policy.RequireRole("Administrator")));

            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
                cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(UnhandledExceptionBehaviour<,>));
                cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
            });

            return services;
        }
    }
}
