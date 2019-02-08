using System.Threading;
using System.Threading.Tasks;
using Common.Info;
using Common.Interface;
using MediatR;

namespace Common.Commands {
    public class DeleteAsyncHandler<TDto>
        : IRequestHandler<DeleteAsyncCommand<TDto>, bool>

        where TDto : DTO, new() {

        private readonly IUserAppService _userAppService;

        public DeleteAsyncHandler(IUserAppService userAppService) {
            _userAppService = userAppService;
        }

        /// <summary>
        /// Handler which processes the command when
        /// user executes a getUserAsync task from app
        /// </summary>
        /// <param name="command"></param>
        /// <param name="command"></param>
        /// <returns name="UserModel"></returns>

        public async Task<bool> Handle(DeleteAsyncCommand<TDto> request,
            CancellationToken cancellationToken) {
            long id = await _userAppService.FindIdByBkAsync((string)request.Bk);
            return await _userAppService.RemoveUserAsync(id);
        }
    }

}
