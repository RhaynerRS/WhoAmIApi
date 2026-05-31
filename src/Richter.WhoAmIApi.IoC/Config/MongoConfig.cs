using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using MongoDB.Driver.Core.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Richter.WhoAmIApi.IoC.Config
{
    public static class MongoConfig
    {
        public static IServiceCollection AddMongo(this IServiceCollection services, IConfiguration configuration)
        {
            string connectionString = configuration.GetConnectionString("DefaultConnection");
            string database = configuration.GetValue<string>("MongoDB:DatabaseName");
            MongoClient client = new(connectionString);

            services.AddSingleton<IMongoClient>(client);
            services.AddSingleton<IMongoDatabase>(client.GetDatabase(database));

            return services;
        }
    }
}
