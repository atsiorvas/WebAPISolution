using System.Threading;
using System.Threading.Tasks;
using Common.Interface;
using MediatR;

namespace Common.Commands {
    public class AddUserWithNotesHandlerAsync
        : IRequestHandler<AddUserWithNotesAsyncCommand, UserModel> {

        private readonly IUserAppService _userAppService;

        public AddUserWithNotesHandlerAsync(IUserAppService userAppService) {
            _userAppService = userAppService;
        }

        /// <summary>
        /// Handler which processes the command when
        /// user executes a getUserAsync task from app
        /// </summary>
        /// <param name="command"></param>
        /// <param name="command"></param>
        /// <returns name="UserModel"></returns>

        public Task<UserModel> Handle(AddUserWithNotesAsyncCommand request,
            CancellationToken cancellationToken) {
            //return await _userAppService.SaveUserAsync(request.userModel);
            throw new System.NotImplementedException();
        }
    }

}
