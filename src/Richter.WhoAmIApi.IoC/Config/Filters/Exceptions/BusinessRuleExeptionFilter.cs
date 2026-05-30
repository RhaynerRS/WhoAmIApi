using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Richter.WhoAmIApi.CrossCutting.Attributes;
using Askmethat.Aspnet.JsonLocalizer.Localizer;
using Richter.WhoAmIApi.CrossCutting.Extensions;
using Richter.WhoAmIApi.CrossCutting.Exceptions;

namespace Richter.WhoAmIApi.IoC.Config.Filters.Exceptions
{
    public class BusinessRuleExeptionFilter(IJsonStringLocalizer<BusinessRuleExeptionFilter> localizer) : IAsyncExceptionFilter
    {
        public Task OnExceptionAsync(ExceptionContext context)
        {
            var exception = context.Exception.InnerException ?? context.Exception;

            if (exception is RegraDeNegocioException)
            {
                var message = exception.Message;

                // Realiza a transcrição da mensagem de negócio caso ela tenha sido internacionalida.
                if (exception.GetType().TryGetCustomAttribute<LocalizerTagAttribute>(out var attribute))
                    message = GetTranslatedMessage(exception, attribute);

                context.HttpContext.Response.StatusCode = 400;

                context.Result = new JsonResult(new
                {
                    Message = message,
                    Tipo = exception.GetType().Name
                });
            }

            return Task.CompletedTask;
        }

        private string GetTranslatedMessage(Exception exception, LocalizerTagAttribute localizerAttribute)
        {
            if (localizerAttribute.HasArguments && exception.Data != null)
            {
                object[] arguments = [.. localizerAttribute.ArgumentKeys
                    .Where(exception.Data.Contains)
                    .Select(key => exception.Data[key]!)];

                return localizer[localizerAttribute.Tag, arguments];
            }

            return localizer[localizerAttribute.Tag];
        }
    }
}