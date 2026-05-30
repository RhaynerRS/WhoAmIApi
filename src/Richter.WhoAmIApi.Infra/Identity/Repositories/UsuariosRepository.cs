using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Richter.WhoAmIApi.CrossCutting.DTO;
using Richter.WhoAmIApi.CrossCutting.Exceptions;
using Richter.WhoAmIApi.Domain.Identity.Entities;
using Richter.WhoAmIApi.Domain.Identity.Repositories;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Richter.WhoAmIApi.Infra.Identity.Repositories
{
    public class UsuariosRepository(UserManager<UsuarioAplicacao> userManager, IOptions<JwtSettingsDto> jwtSettings) : IUsuariosRepository
    {
        private readonly JwtSettingsDto _jwtSettings = jwtSettings.Value;

        public async Task<UsuarioAplicacao> BuscarPorEmailAsync(string email)
        {
            return await userManager.FindByEmailAsync(email);
        }

        public async Task InserirAsync(UsuarioAplicacao usuario, string senha)
        {
            IdentityResult resultado = await userManager.CreateAsync(usuario, senha);
            if (!resultado.Succeeded)
            {
                string erros = string.Join(", ", resultado.Errors.Select(e => e.Description));
                throw new RegraDeNegocioException($"Falha ao criar usuário: {erros}");
            }
        }

        public async Task<bool> VerificarSenhaAsync(UsuarioAplicacao usuario, string senha)
        {
            return await userManager.CheckPasswordAsync(usuario, senha);
        }

        public async Task<IList<string>> ListarRolesAsync(UsuarioAplicacao usuario)
        {
            return await userManager.GetRolesAsync(usuario);
        }

        public JwtCreationDto GerarToken(UsuarioAplicacao usuario, IList<string> roles)
        {
            SymmetricSecurityKey key = new(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));
            SigningCredentials credentials = new(key, SecurityAlgorithms.HmacSha256);
            DateTime expiracao = DateTime.UtcNow.AddHours(_jwtSettings.ExpiracaoHoras);

            List<Claim> claims =
            [
                new(JwtRegisteredClaimNames.Sub, usuario.Id),
                new(JwtRegisteredClaimNames.Email, usuario.Email!),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            ];

            foreach (string role in roles)
                claims.Add(new Claim(ClaimTypes.Role, role));

            JwtSecurityToken token = new(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: expiracao,
                signingCredentials: credentials
            );

            return new JwtCreationDto
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiracao = expiracao,
            };
        }
    }
}
