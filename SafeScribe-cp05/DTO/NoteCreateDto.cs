using System.ComponentModel.DataAnnotations;

namespace SafeScribe_cp05.DTO
{
    public class NoteCreateDto
    {
        [Required(ErrorMessage = "O título é obrigatório.")]
        [StringLength(100)]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "O conteúdo é obrigatório.")]
        public string Content { get; set; } = string.Empty;
    }
}