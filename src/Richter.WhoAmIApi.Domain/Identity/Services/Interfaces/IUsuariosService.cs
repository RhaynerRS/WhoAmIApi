using Richter.WhoAmIApi.Domain.Identity.Entities;

namespace Richter.WhoAmIApi.Domain.Identity.Services.Interfaces
{
    public interface IUsuariosService
    {
        Task<UsuarioAplicacao> InstanciarAsync(string nomeCompleto, string email);
        Task<UsuarioAplicacao> ValidarAsync(string email);
        void Atualizar(UsuarioAplicacao usuario, string nomeCompleto);
    }
}
