using Richter.WhoAmIApi.Application.Auth;
using Richter.WhoAmIApi.Application.Auth.Dtos;
using Richter.WhoAmIApi.CrossCutting.Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Richter.WhoAmIApi.Infra.Identity
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly JwtSettings _jwtSettings;

        public AuthService(UserManager<ApplicationUser> userManager, IOptions<JwtSettings> jwtSettings)
        {
            _userManager = userManager;
            _jwtSettings = jwtSettings.Value;
        }

        public async Task RegisterAsync(RegisterRequest request)
        {
            var usuarioExistente = await _userManager.FindByEmailAsync(request.Email);

            if (usuarioExistente is not null)
                throw new BusinessRuleException("Já existe um usuário cadastrado com este e-mail.");

            var usuario = new ApplicationUser
            {
                UserName = request.Email,
                Email = request.Email,
                NomeCompleto = request.NomeCompleto,
                EmailConfirmed = true,
            };

            var resultado = await _userManager.CreateAsync(usuario, request.Senha);

            if (!resultado.Succeeded)
            {
                var erros = string.Join(", ", resultado.Errors.Select(e => e.Description));
                throw new BusinessRuleException($"Falha ao criar usuário: {erros}");
            }
        }

        public async Task<AuthResponse> LoginAsync(LoginRequest request)
        {
            var usuario = await _userManager.FindByEmailAsync(request.Email)
                ?? throw new BusinessRuleException("E-mail ou senha inválidos.");

            var senhaValida = await _userManager.CheckPasswordAsync(usuario, request.Senha);

            if (!senhaValida)
                throw new BusinessRuleException("E-mail ou senha inválidos.");

            var roles = await _userManager.GetRolesAsync(usuario);
            return GerarToken(usuario, roles);
        }

        private AuthResponse GerarToken(ApplicationUser usuario, IList<string> roles)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expiracao = DateTime.UtcNow.AddHours(_jwtSettings.ExpiracaoHoras);

            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Sub, usuario.Id),
                new(JwtRegisteredClaimNames.Email, usuario.Email!),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            foreach (var role in roles)
                claims.Add(new Claim(ClaimTypes.Role, role));

            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: expiracao,
                signingCredentials: credentials
            );

            return new AuthResponse
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiracao = expiracao,
            };
        }
    }
}
