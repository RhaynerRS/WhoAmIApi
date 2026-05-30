using Askmethat.Aspnet.JsonLocalizer.Extensions;
using Askmethat.Aspnet.JsonLocalizer.JsonOptions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.DependencyInjection;
using System.Globalization;

namespace Richter.WhoAmIApi.IoC.Config
{
    public static class LocalizationConfig
    {
        private static readonly CultureInfo CulturaPadrao = new("pt-BR");
        private static readonly HashSet<CultureInfo> CulturasSuportadas = [new("en-US"), new("pt-BR")];

        public static IServiceCollection AddCustomLocalization(this IServiceCollection services)
        {
            services.AddJsonLocalization(options => {
                options.ResourcesPath = "localization";
                options.LocalizationMode = LocalizationMode.I18n;
            });

            services.Configure<RequestLocalizationOptions>(options =>
            {
                options.RequestCultureProviders = [new AcceptLanguageHeaderRequestCultureProvider()];
                options.DefaultRequestCulture = new RequestCulture(CulturaPadrao);
                options.SupportedCultures = [.. CulturasSuportadas];
                options.SupportedUICultures = [.. CulturasSuportadas];                
            });

            return services;
        }
    }
}