using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System.Reflection;

namespace Richter.WhoAmIApi.IoC.Config
{
    public static class HostConfig
    {
        public static WebApplicationBuilder ConfigureHost(this WebApplicationBuilder builder)
        {
            var compilePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!;
            var port = $"http://*:{builder.Configuration["Application:Porta"]}";

            builder.Configuration
                .AddJsonFile($"{compilePath}/shared/appsettings.json", false)
                .AddJsonFile($"{compilePath}/shared/appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", false);

            builder.WebHost.UseUrls(port);

            return builder;
        }
    }
}