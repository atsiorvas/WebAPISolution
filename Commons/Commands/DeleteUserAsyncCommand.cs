using MediatR;

namespace Common.Commands {
    public class DeleteUserAsyncCommand : IRequest<bool> {
        public string Email;

        public DeleteUserAsyncCommand(string email) {
            Email = email;
        }
    }
}
