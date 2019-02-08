using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Common;
using Common.Info;
using Common.Interface;
using Microsoft.EntityFrameworkCore;

namespace Repository {
    [Obsolete("use UnityOfWork instead", false)]
    public class NoteRepository : INoteRepository {

        private readonly UserContext _context;
        private readonly IConfigurationProvider _cfg;
        private readonly IMapper _mapper;
        public ISaveChangesWarper SaveChangesWarper {

            get {
                return _context;
            }
        }

        public NoteRepository(
            UserContext context, IConfigurationProvider cfg,
            IMapper mapper) {
            _context = context ?? throw new ArgumentNullException();
            _cfg = cfg ?? throw new ArgumentNullException();
            _mapper = mapper ?? throw new ArgumentNullException();
        }

        public async Task<List<NotesModel>> SaveNotesByAsync(string email, List<NotesModel> newNotes) {
            try {
                DbSet<User> users = _context.User;

                User user = _context.User
                    .Where(u => u.Email == email)
                    .Include(u => u.Note).First();

                List<Notes> newNotesToMap = new List<Notes>();

                newNotes.ForEach(note =>
                     newNotesToMap.Add(_mapper.Map<NotesModel, Notes>(note)));

                user.Note.UnionWith(newNotesToMap);

                users.Update(user);
                await _context.SaveChangesAsync();

                return newNotes;
            } catch (Exception) {
                return null;
            }
        }

        public Task<NotesModel> GetNotesByAsync(string email) {
            throw new NotImplementedException();
        }

        public Task<bool> ModifyNoteByAsync(string email) {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteNoteBy(string email) {
            throw new NotImplementedException();
        }
    }
}