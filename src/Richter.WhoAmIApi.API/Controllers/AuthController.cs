using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Richter.WhoAmIApi.Application.Identity.Services.Interfaces;
using Richter.WhoAmIApi.Application.Identity.DataModule.Requests;
using Richter.WhoAmIApi.CrossCutting.DTO;

namespace Richter.WhoAmIApi.API.Controllers
{
    [ApiController]
    [Route("auth")]
    public class AuthController(IAutenticacaoService autenticacaoService) : ControllerBase
    {
        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UsuarioCadastroRequest request)
        {
            await autenticacaoService.RegistrarAsync(request.NomeCompleto, request.Email, request.Senha);
            return Created();
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult<JwtCreationDto>> Login([FromBody] UsuarioLoginRequest request)
        {
            JwtCreationDto response = await autenticacaoService.LoginAsync(request.Email, request.Senha);
            return Ok(response);
        }
    }
}
