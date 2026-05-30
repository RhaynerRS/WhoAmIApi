using Richter.WhoAmIApi.IoC.Config.Filters.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace Richter.WhoAmIApi.IoC.Config.Filters
{
    public static class FiltersConfig
    {
        public static IServiceCollection AddRegraNecocioExceptionFilter(this IServiceCollection services)
        {
            services.Configure<MvcOptions>(options =>
            {
                options.Filters.Add<BusinessRuleExeptionFilter>();
            });

            return services;
        }
    }
}