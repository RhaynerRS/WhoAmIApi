using Richter.WhoAmIApi.Application;
using Mapster;
using Microsoft.Extensions.DependencyInjection;

namespace Richter.WhoAmIApi.IoC.Config
{
    public static class MapsterConfig
    {
        public static IServiceCollection AddMapsterConfig(this IServiceCollection services)
        {
            var config = TypeAdapterConfig.GlobalSettings;

            config.Scan(typeof(IApplication).Assembly);
            config.Compile();

            return services;
        }
    }
}