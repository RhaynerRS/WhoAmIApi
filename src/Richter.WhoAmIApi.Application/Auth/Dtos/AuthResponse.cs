namespace Richter.WhoAmIApi.Application.Auth.Dtos
{
    public class AuthResponse
    {
        public string Token { get; set; } = string.Empty;
        public DateTime Expiracao { get; set; }
    }
}
