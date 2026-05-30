using Richter.WhoAmIApi.IoC.Config;
using Richter.WhoAmIApi.IoC.Config.Filters;
using Richter.WhoAmIApi.IoC.Config.Identity;
using Richter.WhoAmIApi.IoC.Config.Swagger;
using Richter.WhoAmIApi.IoC.Settings;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.FeatureManagement;

namespace Richter.WhoAmIApi.IoC
{
    public static class NativeInjectorBootStrapper
    {
        public static Task AddCommonServices(this IServiceCollection services, IConfiguration configuration, IHostEnvironment environment)
        {
            var serilogSettings = configuration.GetSection("SerilogExtensions").Get<SerilogSettings>()!;

            services.Configure<ApplicationSettings>(configuration.GetSection("Application"));
            services.Configure<ApiBehaviorOptions>(options => options.SuppressModelStateInvalidFilter = true);

            services.AddIdentityConfiguration(configuration);

            services.AddRegraNecocioExceptionFilter();
            services.AddRouting(options => options.LowercaseUrls = true);
            services.AddSerilog(environment, serilogSettings);
            services.AddVersioning();
            services.AddSwagger();  
            services.AddHealthChecks(serilogSettings);
            services.AddOpenTelemetry(configuration, environment);
            services.AddCustomLocalization();
            services.AddMapsterConfig();

            services.InjetarDependenciasApplication();
            services.InjetarDependenciasDomain();
            services.InjetarDependenciasInfra();

            return Task.CompletedTask;
        }

        public static WebApplication UseCommonAppConfiguration(this WebApplication app)
        {
            var applicationSettings = app.Services.GetRequiredService<IOptions<ApplicationSettings>>().Value;

            if (app.Environment.IsDevelopment())
                app.UseDeveloperExceptionPage();

            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.All
            });

            app.UsePathBase(applicationSettings.PathBase);
            app.UseRequestLocalization();
            app.MapHealthChecks("/health");
            app.ConfigureSwagger(applicationSettings);
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();

            return app;
        }
    }
}