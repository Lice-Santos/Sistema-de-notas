using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using SafeScribe_cp05.Models;
using SafeScribe_cp05.Enum;
using SafeScribe_cp05.Interface;

namespace SafeScribe_cp05.Services
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;

        // Banco simulado em memória
        private static readonly List<User> _users = new()
        {
            new User
            {
                Id = Guid.NewGuid().ToString(),
                Username = "admin",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("123456"),
                Role = UserRole.Admin
            }
        };

        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // LOGIN por username
        public Task<string> LoginAndGenerateTokenAsync(string username, string password)
        {
            var user = _users.FirstOrDefault(u =>
                u.Username.Equals(username, StringComparison.OrdinalIgnoreCase));

            if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
                return Task.FromResult(string.Empty);

            var token = GenerateToken(user.Id, user.Username, user.Role);
            return Task.FromResult(token);
        }

        // REGISTRO de novo usuário
        public Task<bool> RegisterUserAsync(string username, string password, UserRole role)
        {
            if (_users.Any(u => u.Username.Equals(username, StringComparison.OrdinalIgnoreCase)))
                return Task.FromResult(false);

            var newUser = new User
            {
                Id = Guid.NewGuid().ToString(),
                Username = username,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(password),
                Role = role
            };

            _users.Add(newUser);
            return Task.FromResult(true);
        }

        // GERAÇÃO do token JWT
        public string GenerateToken(string userId, string username, UserRole role)
        {
            var key = _configuration["Jwt:Key"] ?? throw new InvalidOperationException("Jwt:Key not found.");
            var issuer = _configuration["Jwt:Issuer"];
            var audience = _configuration["Jwt:Audience"];

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, userId),
                new Claim(ClaimTypes.Role, role.ToString())
            };

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.Now.AddHours(2),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
