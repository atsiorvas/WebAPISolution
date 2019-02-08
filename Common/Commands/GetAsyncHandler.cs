using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Common.Info;
using Common.Interface;
using MediatR;

namespace Common.Commands {
    public class GetAsyncHandler<TDto>
        : IRequestHandler<GetCommandAsync<TDto>, UserModel>
        where TDto : DTO, new() {

        private readonly IUserAppService _userAppService;
        public GetAsyncHandler(IUserAppService userAppService) {
            _userAppService = userAppService;
        }

        /// <summary>
        /// Handler which processes the command when
        /// user executes a getUserAsync task from app
        /// </summary>
        /// <param name="command"></param>
        /// <param name="command"></param>
        /// <returns name="UserModel"></returns>

        public async Task<UserModel> Handle(GetCommandAsync<TDto> request,
            CancellationToken cancellationToken) {
            return await _userAppService
                .GetUserWithNotesAsync((string)request.Bk);
        }
    }
}