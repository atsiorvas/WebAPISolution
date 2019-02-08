using System.Threading;
using System.Threading.Tasks;
using Common.Interface;
using MediatR;

namespace Common.Commands {
    public class FileNotificationHandler
    : INotificationHandler<LoggerEvent> {

        private readonly INoteService _noteService;

        public FileNotificationHandler(INoteService noteService) {
            _noteService = noteService;
        }

        public async Task Handle(LoggerEvent notification,
            CancellationToken cancellationToken) {

            await _noteService.SaveToFileAsync(notification.notes);
        }
    }
}