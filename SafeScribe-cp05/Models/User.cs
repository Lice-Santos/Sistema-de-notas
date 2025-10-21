using SafeScribe_cp05.Enum;

namespace SafeScribe_cp05.Models
{
    public class User
    {
        // O Id deve ser string para ser consistente com Guid.NewGuid().ToString() no TokenService
        public string Id { get; set; } = string.Empty;

        // Nome de usuário para login
        public string Username { get; set; } = string.Empty;

        // O hash da senha, gerado e armazenado pelo BCrypt
        public string PasswordHash { get; set; } = string.Empty;

        // A função do usuário (Role) deve ser o ENUM no lado do domínio para segurança e tipagem forte.
        public UserRole Role { get; set; }
    }
}