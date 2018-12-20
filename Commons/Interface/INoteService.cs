using System.Collections.Generic;
using System.Threading.Tasks;

namespace Common {
    public interface INoteService {

        Task<List<NotesModel>> SaveNotesByAsync(string email, List<NotesModel> notes);

        Task<NotesModel> GetNotesByAsync(string email);

        Task<List<NotesModel>> ModifyNoteByAsync(string email, NotesModel noteChanges);
        Task SaveToFileAsync(NotesModel note);
        Task<bool> DeleteNoteBy(string email);
    }
}