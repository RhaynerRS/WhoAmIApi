using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Richter.WhoAmIApi.Application.Identity.Services.Interfaces;
using Richter.WhoAmIApi.Application.Identity.DataModule.Requests;
using Richter.WhoAmIApi.CrossCutting.DTO;

namespace Richter.WhoAmIApi.API.Controllers
{
    [ApiController]
    [Route("auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAutenticacaoService _autenticacaoService;

        public AuthController(IAutenticacaoService autenticacaoService)
        {
            _autenticacaoService = autenticacaoService;
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UsuarioCadastroRequest request)
        {
            await _autenticacaoService.RegistrarAsync(request.NomeCompleto, request.Email, request.Senha);
            return Created();
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UsuarioLoginRequest request)
        {
            JwtCreationDto response = await _autenticacaoService.LoginAsync(request.Email, request.Senha);
            return Ok(response);
        }
    }
}
