using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Richter.WhoAmIApi.IoC.Config.Swagger.Filters
{
    public class SwaggerRemoveReadonlyPropertyParametersFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (context.ApiDescription?.ParameterDescriptions == null)
                return;

            var parametersReadyOnly = context.ApiDescription.ParameterDescriptions.Where(pd =>
                pd.ModelMetadata?.IsReadOnly == true && !pd.IsRequired && pd.ModelMetadata?.BindingSource == null);

            foreach (var parameterToHide in parametersReadyOnly)
            {
                var parameter = operation.Parameters
                    .First(p => p.Name.Equals(parameterToHide.Name, StringComparison.InvariantCultureIgnoreCase));

                operation.Parameters.Remove(parameter);
            }
        }
    }
}