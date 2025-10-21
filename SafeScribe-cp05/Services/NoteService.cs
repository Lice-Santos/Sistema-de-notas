using SafeScribe_cp05.Interface;
using SafeScribe_cp05.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SafeScribe_cp05.Services
{
    public class NoteService : INoteService
    {
        // Simulação de banco de dados
        private static readonly List<Note> _notes = new List<Note>();

        // Métodos ajustados para usar string para ID e UserId

        public Task<Note?> GetNoteByIdAsync(string id)
        {
            var note = _notes.FirstOrDefault(n => n.Id == id);
            return Task.FromResult(note);
        }

        public Task<List<Note>> GetAllNotesAsync()
        {
            return Task.FromResult(_notes.ToList());
        }

        public Task<Note> CreateNoteAsync(Note newNote)
        {
            // Gera um GUID para o ID, já que agora é string
            newNote.Id = Guid.NewGuid().ToString();
            newNote.CreatedAt = DateTime.Now;
            _notes.Add(newNote);
            return Task.FromResult(newNote);
        }

        public Task<bool> UpdateNoteAsync(Note noteToUpdate)
        {
            var existingNote = _notes.FirstOrDefault(n => n.Id == noteToUpdate.Id);
            if (existingNote == null) return Task.FromResult(false);

            existingNote.Title = noteToUpdate.Title;
            existingNote.Content = noteToUpdate.Content;
            // UserId e CreatedAt não devem ser alterados
            return Task.FromResult(true);
        }

        public Task<bool> DeleteNoteAsync(string id)
        {
            var count = _notes.RemoveAll(n => n.Id == id);
            return Task.FromResult(count > 0);
        }
    }
}