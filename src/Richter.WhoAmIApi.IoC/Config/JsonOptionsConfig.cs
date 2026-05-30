using Microsoft.Extensions.DependencyInjection;
using System.Text.Json.Serialization;

namespace Richter.WhoAmIApi.IoC.Config
{
    public static class JsonOptionsConfig
    {
        public static IMvcBuilder AddCustomJsonOptions(this IMvcBuilder builder)
        {
            return builder.AddJsonOptions(op =>
            {
                op.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                op.JsonSerializerOptions.PropertyNamingPolicy = null;
            });
        }
    }
}