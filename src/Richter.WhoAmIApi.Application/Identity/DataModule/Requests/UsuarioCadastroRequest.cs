namespace Richter.WhoAmIApi.Application.Identity.DataModule.Requests
{
    public class UsuarioCadastroRequest
    {
        public string NomeCompleto { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Senha { get; set; } = string.Empty;
    }
}
