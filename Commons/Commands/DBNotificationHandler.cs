using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace Common.Commands {
    public class DBNotificationHandler
        : INotificationHandler<LoggerEvent> {

        private readonly INoteService _noteService;

        public DBNotificationHandler(INoteService noteService) {
            _noteService = noteService;
        }
        public async Task Handle(LoggerEvent notification,
            CancellationToken cancellationToken) {

            await _noteService.ModifyNoteByAsync(notification.email,
               notification.notes);
        }
    }
}