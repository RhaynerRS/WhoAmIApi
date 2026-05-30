using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;
using Richter.WhoAmIApi.IoC.Settings;
using Richter.WhoAmIApi.IoC.Config.Swagger.Filters;

namespace Richter.WhoAmIApi.IoC.Config.Swagger
{
    public static class SwaggerConfig
    {
        public static IServiceCollection AddSwagger(this IServiceCollection services)
        {
            services.AddTransient<IConfigureOptions<SwaggerGenOptions>, SwaggerOptions>();

            services.AddSwaggerGen(options =>
            {
                options.DescribeAllParametersInCamelCase();
                options.CustomSchemaIds(x => x.FullName);

                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Name = "Authorization",
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    Description = "Insira o token JWT aqui sem nenhum tipo de prefixo.",
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference { Id = "Bearer", Type = ReferenceType.SecurityScheme }
                        },
                        Array.Empty<string>()
                    }
                });

                options.OrderActionsBy((action) => action.RelativePath);
                options.OperationFilter<SwaggerRemoveReadonlyPropertyParametersFilter>();
            });

            return services;
        }

        public static WebApplication ConfigureSwagger(this WebApplication app, ApplicationSettings settings)
        {
            var versoes = app.DescribeApiVersions();

            app.UseSwaggerStaticFiles();
            app.ConfigurarRedirecionamentosSwagger(settings);
            app.UseSwagger();

            app.UseSwaggerUI(options =>
            {
                options.EnablePersistAuthorization();
                options.EnableTryItOutByDefault();
                options.DefaultModelsExpandDepth(0);
                options.DefaultModelExpandDepth(5);
                options.DisplayRequestDuration();

                options.HeadContent = ObterHeadContent(app);

                foreach (var groupName in versoes.Select(d => d.GroupName))
                {
                    var url = $"../swagger/{groupName}/swagger.json";
                    var name = groupName.ToUpperInvariant();

                    options.SwaggerEndpoint(url, name);
                }

                options.InjectStylesheet($"../swagger/assets/swagger-custom-ui.css");
                options.InjectJavascript($"../swagger/assets/swagger-custom-ui.js", "text/javascript");
            });

            return app;
        }

        private static void UseSwaggerStaticFiles(this WebApplication app)
        {
            var diretorioCompilacao = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!;

            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(diretorioCompilacao, "assets/swagger")),
                RequestPath = "/swagger/assets"
            });
        }

        private static string ObterHeadContent(WebApplication app)
        {
            var environmentFormatado = app.Environment.EnvironmentName switch
            {
                "Development" => "Desenvolvimento",
                "Hml" => "Homologação",
                "Prod" => "Produção",
                _ => "N/A",
            };

            var environmentTag = app.Environment.EnvironmentName.ToLower();
            var dataBuild = DateTime.Now.ToString("dd-MM-yyyy HH:mm");

            return $@"<div class=""environment-header {environmentTag}""><div>{environmentFormatado} - {dataBuild}</div></div>";
        }

        private static void ConfigurarRedirecionamentosSwagger(this WebApplication app, ApplicationSettings settings)
        {
            var paths = new string[]
            {
                "/",
                "/index.html",
                $"{settings.PathBase}",
                $"{settings.PathBase}/index.html"
            };

            foreach (var path in paths)
                app.MapGet(path, () => Results.Redirect($"{settings.PathBase}/swagger/index.html")).ExcludeFromDescription();
        }
    }
}