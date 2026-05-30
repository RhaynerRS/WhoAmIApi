using Richter.WhoAmIApi.CrossCutting.Exceptions;
using Richter.WhoAmIApi.Domain.Identity.Entities;
using Richter.WhoAmIApi.Domain.Identity.Repositories;
using Richter.WhoAmIApi.Domain.Identity.Services.Interfaces;

namespace Richter.WhoAmIApi.Domain.Identity.Services
{
    public class UsuariosService : IUsuariosService
    {
        private readonly IUsuariosRepository _usuariosRepository;

        public UsuariosService(IUsuariosRepository usuariosRepository)
        {
            _usuariosRepository = usuariosRepository;
        }

        public async Task<UsuarioAplicacao> InstanciarAsync(string nomeCompleto, string email)
        {
            UsuarioAplicacao? usuarioExistente = await _usuariosRepository.BuscarPorEmailAsync(email);
            if (usuarioExistente is not null)
                throw new RegraDeNegocioException("Já existe um usuário cadastrado com este e-mail.");

            return new UsuarioAplicacao(nomeCompleto, email);
        }

        public async Task<UsuarioAplicacao> ValidarAsync(string email)
        {
            UsuarioAplicacao? usuario = await _usuariosRepository.BuscarPorEmailAsync(email);
            if (usuario is null)
                throw new RegraDeNegocioException("Usuário não encontrado.");

            return usuario;
        }

        public void Atualizar(UsuarioAplicacao usuario, string nomeCompleto)
        {
            usuario.SetNomeCompleto(nomeCompleto);
        }
    }
}
