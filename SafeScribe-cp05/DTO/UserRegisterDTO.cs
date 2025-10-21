using SafeScribe_cp05.Enum;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace SafeScribe_cp05.DTO
{
    public class UserRegisterDTO
    {
        [Required(ErrorMessage = "O nome de usuário é obrigatório.")]
        [JsonPropertyName("username")]
        public string Username { get; set; } = string.Empty;

        [Required(ErrorMessage = "A senha é obrigatória.")]
        [MinLength(6, ErrorMessage = "A senha deve ter no mínimo 6 caracteres.")]
        [JsonPropertyName("password")]
        public string Password { get; set; } = string.Empty;

        // A Role é opcional ou definida pelo cliente no registro.
        [JsonConverter(typeof(JsonStringEnumConverter))]
        [JsonPropertyName("role")]
        public UserRole Role { get; set; } = UserRole.Leitor;
    }
}
