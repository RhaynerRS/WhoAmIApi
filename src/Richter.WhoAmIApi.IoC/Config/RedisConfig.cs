using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Richter.WhoAmIApi.IoC.Settings;
using StackExchange.Redis;

namespace Richter.WhoAmIApi.IoC.Config
{
    public static class RedisConfig
    {
        public static IServiceCollection AddRedis(this IServiceCollection services, IConfiguration configuration)
        {
            var redisSettings = configuration.GetSection("Redis").Get<RedisSettings>()
                ?? throw new InvalidOperationException("A seção 'Redis' não foi encontrada no appsettings.");

            services.Configure<RedisSettings>(configuration.GetSection("Redis"));

            var configOptions = ConfigurationOptions.Parse(redisSettings.ConnectionString);
            configOptions.ConnectTimeout = redisSettings.ConnectTimeout;
            configOptions.SyncTimeout = redisSettings.SyncTimeout;
            configOptions.AbortOnConnectFail = true;

            var multiplexer = ConnectionMultiplexer.Connect(configOptions);

            services.AddSingleton<IConnectionMultiplexer>(multiplexer);
            services.AddSingleton<IDatabase>(sp =>
                sp.GetRequiredService<IConnectionMultiplexer>()
                  .GetDatabase(redisSettings.DatabaseIndex));

            return services;
        }
    }
}
