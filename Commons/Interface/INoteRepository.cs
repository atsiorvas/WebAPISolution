using Common.Interface;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Common {
    public interface INoteRepository : IRepository<Notes> {

        Task<List<NotesModel>> SaveNotesByAsync(string email, List<NotesModel> notes);

        Task<NotesModel> GetNotesByAsync(string email);

        Task<bool> ModifyNoteByAsync(string email);

        Task<bool> DeleteNoteBy(string email);
    }
}