using SafeScribe_cp05.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SafeScribe_cp05.Interface
{
    // Id agora é string
    public interface INoteService
    {
        Task<Note?> GetNoteByIdAsync(string id); // Id como string
        Task<List<Note>> GetAllNotesAsync();
        Task<Note> CreateNoteAsync(Note newNote);
        Task<bool> UpdateNoteAsync(Note noteToUpdate);
        Task<bool> DeleteNoteAsync(string id); // Id como string
    }
}