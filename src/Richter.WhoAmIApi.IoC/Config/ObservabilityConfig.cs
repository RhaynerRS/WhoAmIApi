using Richter.WhoAmIApi.IoC.Settings;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace Richter.WhoAmIApi.IoC.Config
{
    public static class ObservabilityConfig
    {
        public static IServiceCollection AddHealthChecks(this IServiceCollection services,
                                                         SerilogSettings serilogSettings)
        {
            services.AddHealthChecks();

            return services;
        }

        public static IServiceCollection AddOpenTelemetry(this IServiceCollection services, IConfiguration configuration, IHostEnvironment environment)
        {
            return services;
        }
    }
}