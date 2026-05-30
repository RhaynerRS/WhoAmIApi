using Richter.WhoAmIApi.CrossCutting.DTO;
using Richter.WhoAmIApi.Domain.Identity.Entities;

namespace Richter.WhoAmIApi.Domain.Identity.Repositories
{
    public interface IUsuariosRepository
    {
        Task<UsuarioAplicacao> BuscarPorEmailAsync(string email);
        Task InserirAsync(UsuarioAplicacao usuario, string senha);
        Task<bool> VerificarSenhaAsync(UsuarioAplicacao usuario, string senha);
        Task<IList<string>> ListarRolesAsync(UsuarioAplicacao usuario);
        JwtCreationDto GerarToken(UsuarioAplicacao usuario, IList<string> roles);
    }
}
