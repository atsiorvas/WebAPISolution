using Common.Info;
using MediatR;

namespace Common.Commands {
    public class LoggerEvent : INotification {

        public NotesModel notes;
        public string email;
        public LoggerEvent(NotesModel notes, string email) {
            this.notes = notes;
            this.email = email;
        }
    }
}