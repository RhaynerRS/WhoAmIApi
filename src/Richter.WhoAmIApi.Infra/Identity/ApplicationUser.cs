using Microsoft.AspNetCore.Identity;

namespace Richter.WhoAmIApi.Infra.Identity
{
    public class ApplicationUser : IdentityUser
    {
        public string NomeCompleto { get; set; } = string.Empty;
    }
}
