using SafeScribe_cp05.Enum;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization; // Importante para [JsonPropertyName] e [JsonConverter]

namespace SafeScribe_cp05.DTO
{
    public class UserDTO
    {
        // -------------------------------------------------------------
        // CORREÇÃO 1: Mapeamento de 'username'
        // Se o erro "The user field is required" persistir, 
        // adicione o [JsonPropertyName("user")] se o ASP.NET Core 
        // estiver esperando 'user' no JSON.
        // -------------------------------------------------------------
        [Required(ErrorMessage = "O nome de usuário é obrigatório.")]
        [JsonPropertyName("username")] // Força a leitura do campo 'username' do JSON
        public string Username { get; set; } = string.Empty;

        [Required(ErrorMessage = "A senha é obrigatória.")]
        [JsonPropertyName("password")]
        public string Password { get; set; } = string.Empty;

        // -------------------------------------------------------------
        // CORREÇÃO 2: Conversão de Enum
        // Força o serializador a converter strings para ENUM (e vice-versa)
        // -------------------------------------------------------------
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public UserRole Role { get; set; }
    }
}