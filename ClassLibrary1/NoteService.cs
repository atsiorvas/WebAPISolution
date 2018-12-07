using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Common;
using Repository;

namespace Service {
    public class NoteService : INoteService {
        
        private const bool Eager = true;
        private readonly IConfigurationProvider _cfg;
        private readonly UnitOfWork _unitOfWork;
        private readonly IMapper _mapper;


        public NoteService(IConfigurationProvider confg,
            UnitOfWork unitOfWork,
            IMapper mapper) {
            _cfg = confg;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public Task<bool> DeleteNoteBy(string email) {
            throw new System.NotImplementedException();
        }

        public Task<NotesModel> GetNotesByAsync(string email) {
            throw new System.NotImplementedException();
        }

        public Task<bool> ModifyNoteByAsync(string email) {
            throw new System.NotImplementedException();
        }

        //public async Task<List<NotesModel>>
        //    SaveNotesByAsync(string email, List<NotesModel> notes) {

        //    List<NotesModel> n = await _unitOfWork.NoteRepository.SaveNotesByAsync(email, notes);
        //    return n;
        //}

        public async Task<List<NotesModel>>
            SaveNotesByAsync(string email, List<NotesModel> newNotes) {
            try {

                User user = _unitOfWork.UserRepository.Get(Eager, filter: e => e.Email == email);


                List<Notes> newNotesToMap = new List<Notes>();

                newNotes.ForEach(note =>
                     newNotesToMap.Add(_mapper.Map<NotesModel, Notes>(note)));

                user.Note.AddRange(newNotesToMap);

                await _unitOfWork.UserRepository.UpdateAsync(user);

                return newNotes;
            } catch (Exception ex) {
                return null;
            }
        }

    }
}
