using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SafeScribe_cp05.DTO;
using SafeScribe_cp05.Interface;
using SafeScribe_cp05.Models;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SafeScribe_cp05.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/v1/notas")]
    public class NotasController : ControllerBase
    {
        private readonly INoteService _noteService;

        public NotasController(INoteService noteService)
        {
            _noteService = noteService;
        }

        private (string? UserId, string? Role) GetUserClaims()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var role = User.FindFirst(ClaimTypes.Role)?.Value;
            return (userId, role);
        }

        // ----------------------------------------------------------------------
        // POST /api/v1/notas - Criar Nota
        // ----------------------------------------------------------------------
        [HttpPost]
        [Authorize(Roles = "Editor,Admin")]
        public async Task<IActionResult> CriarNota([FromBody] NoteCreateDto dto)
        {
            var (userId, _) = GetUserClaims();

            if (userId == null)
            {
                return Unauthorized(new { Message = "Token inválido: UserId ausente." });
            }

            var newNote = new Note
            {
                Title = dto.Title,
                Content = dto.Content,
                // UserId agora é String (sem conversão)
                UserId = userId
            };

            var createdNote = await _noteService.CreateNoteAsync(newNote);

            var createdDto = new Note
            {
                Id = createdNote.Id, // Id agora é string
                Title = createdNote.Title,
                Content = createdNote.Content,
                CreatedAt = createdNote.CreatedAt,
                UserId = createdNote.UserId
            };

            return CreatedAtAction(nameof(ObterNota), new { id = createdNote.Id }, createdDto);
        }

        // ----------------------------------------------------------------------
        // GET /api/v1/notas/{id} - Obter Nota
        // O parâmetro da rota é string
        // ----------------------------------------------------------------------
        [HttpGet("{id}")]
        public async Task<IActionResult> ObterNota(string id)
        {
            var (userIdStr, userRole) = GetUserClaims();

            var note = await _noteService.GetNoteByIdAsync(id);

            if (note == null)
            {
                return NotFound();
            }

            // Lógica de Segurança: UserId e Id são strings
            bool isOwner = note.UserId == userIdStr;
            bool isAdmin = userRole == "Admin";

            if (isOwner || isAdmin)
            {
                var dto = new Note { Id = note.Id, Title = note.Title, Content = note.Content, CreatedAt = note.CreatedAt, UserId = note.UserId };
                return Ok(dto);
            }

            return Forbid();
        }

        // ----------------------------------------------------------------------
        // PUT /api/v1/notas/{id} - Atualizar Nota
        // O parâmetro da rota é string
        // ----------------------------------------------------------------------
        [HttpPut("{id}")]
        public async Task<IActionResult> AtualizarNota(string id, [FromBody] NoteCreateDto dto)
        {
            var (userIdStr, userRole) = GetUserClaims();

            var existingNote = await _noteService.GetNoteByIdAsync(id);
            if (existingNote == null) return NotFound();

            bool isOwner = existingNote.UserId == userIdStr;
            bool isAdmin = userRole == "Admin";

            if (isOwner || isAdmin)
            {
                // Verifica se a Role permite a edição (Leitor não pode)
                if (userRole == "Leitor" && !isAdmin)
                {

                    return StatusCode(StatusCodes.Status403Forbidden, new { Message = "Sua função Leitor não permite a edição de notas." });
                }

                existingNote.Title = dto.Title;
                existingNote.Content = dto.Content;

                await _noteService.UpdateNoteAsync(existingNote);
                return NoContent();
            }

            return Forbid();
        }

        // ----------------------------------------------------------------------
        // DELETE /api/v1/notas/{id} - Apagar Nota
        // O parâmetro da rota é string
        // ----------------------------------------------------------------------
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ApagarNota(string id)
        {
            var success = await _noteService.DeleteNoteAsync(id);

            if (!success)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}