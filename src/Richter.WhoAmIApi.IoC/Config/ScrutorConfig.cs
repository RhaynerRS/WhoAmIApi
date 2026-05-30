using Richter.WhoAmIApi.Application;
using Richter.WhoAmIApi.Domain;
using Richter.WhoAmIApi.Infra;
using Microsoft.Extensions.DependencyInjection;
using Scrutor;
using System.Reflection;

namespace Richter.WhoAmIApi.IoC.Config
{
    public static class ScrutorConfig
    {
        public static IServiceCollection InjetarDependenciasApplication(this IServiceCollection services)
        {
            services.ScanServices(typeof(IApplication).Assembly);
            return services;
        }

        public static IServiceCollection InjetarDependenciasDomain(this IServiceCollection services)
        {
            services.ScanServices(typeof(IDomain).Assembly);
            return services;
        }

        public static IServiceCollection InjetarDependenciasInfra(this IServiceCollection services)
        {
            services.ScanServices(typeof(IInfra).Assembly);
            return services;
        }

        private static IServiceCollection ScanServices(this IServiceCollection services, Assembly assemby)
        {
            services.Scan(scan => scan
                .FromAssemblies(assemby)
                .AddClasses()
                .UsingRegistrationStrategy(RegistrationStrategy.Skip)
                .AsImplementedInterfaces(i => i.Namespace!.StartsWith("Richter.WhoAmIApi"))                
                .WithScopedLifetime());

            return services;
        }
    }
}