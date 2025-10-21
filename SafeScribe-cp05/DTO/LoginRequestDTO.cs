using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace SafeScribe_cp05.DTO
{
    public class LoginRequestDTO
    {
        [Required(ErrorMessage = "O nome de usuário é obrigatório.")]
        [JsonPropertyName("username")]
        public string Username { get; set; } = string.Empty;

        [Required(ErrorMessage = "A senha é obrigatória.")]
        [JsonPropertyName("password")]
        public string Password { get; set; } = string.Empty;
    }
}
