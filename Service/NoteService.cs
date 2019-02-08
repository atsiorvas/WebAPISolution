using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Common;
using Common.Info;
using Common.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Repository;

namespace Service {
    public class NoteService : INoteService {

        private const bool Eager = true;
        private readonly IConfigurationProvider _cfg;
        private readonly UnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<NoteService> _logger;
        private readonly UserContext _context = null;


        public NoteService(IConfigurationProvider confg,
            UnitOfWork unitOfWork,
            IMapper mapper,
            ILogger<NoteService> logger,
            UserContext context) {
            _cfg = confg;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
            _context = context;
        }

        public Task<bool> DeleteNoteBy(string email) {
            throw new System.NotImplementedException();
        }

        public Task<NotesModel> GetNotesByAsync(string email) {
            throw new System.NotImplementedException();
        }


        public async Task<List<NotesModel>> ModifyNoteByAsync(string email,
            NotesModel noteChanges) {
            User user = await _unitOfWork.UserRepository
                    .GetAsync(Eager, filter: e => e.Email == email);



            if (user != null) {
                Notes noteMap = _mapper.Map<Notes>(noteChanges);
                foreach (var note in user.Note) {
                    note.Lang = noteMap.Lang;
                    note.Text = noteMap.Text;
                }
                _unitOfWork.UserRepository.Update(user);

                List<NotesModel> newNotesToMap = new List<NotesModel>();

                user.Note.ToList()
                    .ForEach(note =>
                        newNotesToMap.Add(_mapper.Map<NotesModel>(note)));

                return newNotesToMap;
            }
            return null;
        }

        public async Task<List<NotesModel>>
            SaveNotesByAsync(string email, List<NotesModel> newNotes) {

            try {
                User user = await _unitOfWork.UserRepository
                    .GetAsync(Eager, filter: e => e.Email == email);

                if (user == null) {
                    return null;
                }
                List<Notes> ilistDest = _mapper.Map<List<NotesModel>, List<Notes>>(newNotes);

                _context.User.Update(user);
                _context.Entry(user.AuditedEntity).State = EntityState.Modified;

                user.Note.UnionWith(ilistDest);

                await _unitOfWork.SaveChangesAsync();
                return newNotes;
            } catch (Exception ex) {
                _logger.LogError("Exception while saving notes", ex);
                return null;
            }
        }

        public async Task SaveToFileAsync(NotesModel note) {
            string mydocpath =
                Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            // Write the string array to a new file named "WriteLines.txt".
            try {
                using (StreamWriter outputFile
                    = new StreamWriter(Path.Combine(mydocpath, "WriteLines.txt"))) {
                    await outputFile.WriteLineAsync($"Lang = {(char)note.Lang}");
                    await outputFile.WriteLineAsync($"Text= {note.Text}");
                }
            } catch (Exception ex) {
            }
        }
    }
}
