namespace Richter.WhoAmIApi.Application.Identity.DataModule.Requests
{
    public class UsuarioLoginRequest
    {
        public string Email { get; set; } = string.Empty;
        public string Senha { get; set; } = string.Empty;
    }
}
