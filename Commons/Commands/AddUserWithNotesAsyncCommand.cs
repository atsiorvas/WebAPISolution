using MediatR;

namespace Common.Commands {
    public class AddUserWithNotesAsyncCommand : IRequest<UserModel> {
        public UserModel userModel;
        public AddUserWithNotesAsyncCommand(UserModel user) {
            userModel = user;
        }
    }

}
