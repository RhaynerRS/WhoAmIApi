using Richter.WhoAmIApi.Application.Identity.Services.Interfaces;
using Richter.WhoAmIApi.CrossCutting.DTO;
using Richter.WhoAmIApi.CrossCutting.Exceptions;
using Richter.WhoAmIApi.Domain.Identity.Entities;
using Richter.WhoAmIApi.Domain.Identity.Repositories;
using Richter.WhoAmIApi.Domain.Identity.Services.Interfaces;

namespace Richter.WhoAmIApi.Application.Identity.Services
{
    public class AutenticacaoService : IAutenticacaoService
    {
        private readonly IUsuariosService _usuariosService;
        private readonly IUsuariosRepository _usuariosRepository;

        public AutenticacaoService(
            IUsuariosService usuariosService,
            IUsuariosRepository usuariosRepository)
        {
            _usuariosService = usuariosService;
            _usuariosRepository = usuariosRepository;
        }

        public async Task RegistrarAsync(string nomeCompleto, string email, string senha)
        {
            UsuarioAplicacao usuario = await _usuariosService.InstanciarAsync(nomeCompleto, email);
            await _usuariosRepository.InserirAsync(usuario, senha);
        }

        public async Task<JwtCreationDto> LoginAsync(string email, string senha)
        {
            UsuarioAplicacao usuario = await _usuariosService.ValidarAsync(email);

            bool senhaValida = await _usuariosRepository.VerificarSenhaAsync(usuario, senha);
            if (!senhaValida)
                throw new RegraDeNegocioException("E-mail ou senha inválidos.");

            IList<string> roles = await _usuariosRepository.ListarRolesAsync(usuario);
            return _usuariosRepository.GerarToken(usuario, roles);
        }
    }
}
