using SafeScribe_cp05.Enum;

namespace SafeScribe_cp05.Interface
{
    public interface ITokenService
    {
        // Login por username (retorna token JWT)
        Task<string> LoginAndGenerateTokenAsync(string username, string password);

        // Registro de usuário
        Task<bool> RegisterUserAsync(string username, string password, UserRole role);

        // Geração de token JWT
        string GenerateToken(string userId, string username, UserRole role);
    }
}
