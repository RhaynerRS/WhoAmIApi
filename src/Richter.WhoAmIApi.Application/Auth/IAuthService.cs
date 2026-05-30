using Richter.WhoAmIApi.Application.Auth.Dtos;

namespace Richter.WhoAmIApi.Application.Auth
{
    public interface IAuthService
    {
        Task<AuthResponse> LoginAsync(LoginRequest request);
        Task RegisterAsync(RegisterRequest request);
    }
}
