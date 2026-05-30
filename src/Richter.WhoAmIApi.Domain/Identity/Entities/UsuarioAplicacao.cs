using AspNetCore.Identity.MongoDbCore.Models;
using MongoDbGenericRepository.Attributes;
using Richter.WhoAmIApi.CrossCutting.Exceptions;

namespace Richter.WhoAmIApi.Domain.Identity.Entities
{
    [CollectionName("Usuarios")]
    public class UsuarioAplicacao : MongoIdentityUser<string>
    {
        public UsuarioAplicacao() { }

        public UsuarioAplicacao(string nomeCompleto, string email)
        {
            SetNomeCompleto(nomeCompleto);
            UserName = email;
            Email = email;
            EmailConfirmed = true;
        }

        public virtual string NomeCompleto { get; protected set; } = string.Empty;

        public virtual void SetNomeCompleto(string nomeCompleto)
        {
            if (string.IsNullOrWhiteSpace(nomeCompleto))
                throw new RegraDeNegocioException("Nome completo é obrigatório.");
            NomeCompleto = nomeCompleto;
        }
    }
}
