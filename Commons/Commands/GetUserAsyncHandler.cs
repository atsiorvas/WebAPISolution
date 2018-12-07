using System.Threading;
using System.Threading.Tasks;
using Common.Interface;
using MediatR;

namespace Common.Commands {
    public class GetUserAsyncHandler
        : IRequestHandler<GetUserCommandAsync, UserModel> {

        private readonly IUserAppService _userAppService;
        public GetUserAsyncHandler(IUserAppService userAppService) {
            _userAppService = userAppService;
        }

        /// <summary>
        /// Handler which processes the command when
        /// user executes a getUserAsync task from app
        /// </summary>
        /// <param name="command"></param>
        /// <param name="command"></param>
        /// <returns name="UserModel"></returns>
        public async Task<UserModel> Handle(GetUserCommandAsync request,
            CancellationToken cancellationToken) {
            if (request != null && !string.IsNullOrEmpty(request.Email)) {
                return await _userAppService.GetUserWithNotesAsync(request.Email);
            }
            return null;
        }
    }
}
