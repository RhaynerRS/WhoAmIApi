using Richter.WhoAmIApi.CrossCutting.DTO;

namespace Richter.WhoAmIApi.Application.Identity.Services.Interfaces
{
    public interface IAutenticacaoService
    {
        Task<JwtCreationDto> LoginAsync(string email, string senha);
        Task RegistrarAsync(string nomeCompleto, string email, string senha);
    }
}
