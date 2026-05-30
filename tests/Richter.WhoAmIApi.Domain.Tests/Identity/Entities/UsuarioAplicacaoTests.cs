using FluentAssertions;
using Richter.WhoAmIApi.CrossCutting.Exceptions;
using Richter.WhoAmIApi.Domain.Identity.Entities;
using Xunit;

namespace Richter.WhoAmIApi.Domain.Tests.Identity.Entities
{
    public class UsuarioAplicacaoTests
    {
        private readonly UsuarioAplicacao sut;

        public UsuarioAplicacaoTests()
        {
            sut = new UsuarioAplicacao("Nome Teste", "teste@email.com");
        }

        public class Construtor : UsuarioAplicacaoTests
        {
            [Theory]
            [InlineData(null)]
            [InlineData("")]
            [InlineData("   ")]
            public void Quando_NomeCompletoInvalido_Espero_RegraDeNegocioException(string nomeCompleto)
            {
                Action act = () => new UsuarioAplicacao(nomeCompleto, "email@teste.com");

                act.Should().Throw<RegraDeNegocioException>()
                    .WithMessage("*obrigatório*");
            }

            [Fact]
            public void Quando_DadosValidos_Espero_NomeCompletoPreenchido()
            {
                UsuarioAplicacao usuario = new UsuarioAplicacao("João da Silva", "joao@teste.com");

                usuario.NomeCompleto.Should().Be("João da Silva");
            }

            [Fact]
            public void Quando_DadosValidos_Espero_EmailPreenchido()
            {
                UsuarioAplicacao usuario = new UsuarioAplicacao("João da Silva", "joao@teste.com");

                usuario.Email.Should().Be("joao@teste.com");
            }

            [Fact]
            public void Quando_DadosValidos_Espero_EmailConfirmado()
            {
                UsuarioAplicacao usuario = new UsuarioAplicacao("João da Silva", "joao@teste.com");

                usuario.EmailConfirmed.Should().BeTrue();
            }
        }

        public class SetNomeCompletoMetodo : UsuarioAplicacaoTests
        {
            [Theory]
            [InlineData(null)]
            [InlineData("")]
            [InlineData("   ")]
            public void Quando_NomeCompletoInvalido_Espero_RegraDeNegocioException(string nomeCompleto)
            {
                sut.Invoking(x => x.SetNomeCompleto(nomeCompleto))
                    .Should().Throw<RegraDeNegocioException>()
                    .WithMessage("*obrigatório*");
            }

            [Fact]
            public void Quando_NomeCompletoValido_Espero_AtributoPreenchido()
            {
                sut.SetNomeCompleto("Maria Souza");

                sut.NomeCompleto.Should().Be("Maria Souza");
            }
        }
    }
}
