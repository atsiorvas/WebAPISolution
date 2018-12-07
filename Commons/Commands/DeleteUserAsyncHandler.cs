using System.Threading;
using System.Threading.Tasks;
using Common.Interface;
using MediatR;

namespace Common.Commands {
    public class DeleteUserAsyncHandler
        : IRequestHandler<DeleteUserAsyncCommand, bool> {

        private readonly IUserAppService _userAppService;

        public DeleteUserAsyncHandler(IUserAppService userAppService) {
            _userAppService = userAppService;
        }

        /// <summary>
        /// Handler which processes the command when
        /// user executes a getUserAsync task from app
        /// </summary>
        /// <param name="command"></param>
        /// <param name="command"></param>
        /// <returns name="UserModel"></returns>
        /// 
        public Task<bool> Handle(DeleteUserAsyncCommand request, CancellationToken cancellationToken) {
            throw new System.NotImplementedException();
            //if (request != null && !string.IsNullOrEmpty(request.Email)) {
            //    return await _userAppService.RemoveUserAsync(request.Email);
            //}
            //return false;
        }
    }

}
