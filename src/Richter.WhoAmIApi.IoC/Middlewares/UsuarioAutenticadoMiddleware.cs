using Microsoft.AspNetCore.Http;
using Richter.WhoAmIApi.CrossCutting.DTO;
using System.Security.Claims;

namespace Richter.WhoAmIApi.IoC.Middlewares
{
    public class UsuarioAutenticadoMiddleware(RequestDelegate next)
    {
        public async Task InvokeAsync(HttpContext context, UsuarioAutenticadoDto usuarioAutenticado)
        {
            if (context.User.Identity?.IsAuthenticated == true)
            {
                usuarioAutenticado.Id = context.User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;
                usuarioAutenticado.Email = context.User.FindFirstValue(ClaimTypes.Email) ?? string.Empty;
                usuarioAutenticado.UserName = context.User.FindFirstValue(ClaimTypes.Name) ?? string.Empty;
            }

            await next(context);
        }
    }
}
