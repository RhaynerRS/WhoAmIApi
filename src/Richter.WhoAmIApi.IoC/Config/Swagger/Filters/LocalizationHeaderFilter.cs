using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Richter.WhoAmIApi.IoC.Config.Swagger.Filters
{
    public class LocalizationHeaderFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            operation.Parameters ??= [];

            operation.Parameters.Add(new OpenApiParameter
            {
                Name = "Accept-Language",
                In = ParameterLocation.Header,
                Required = true,
                Schema = new OpenApiSchema
                {
                    Type = "string",
                    Enum =
                    [
                        new OpenApiString("en-US"),
                        new OpenApiString("pt-BR"),
                    ],
                    Default = new OpenApiString("pt-BR")
                }
            });
        }
    }
}