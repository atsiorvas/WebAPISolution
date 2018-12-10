using System.Threading;
using System.Threading.Tasks;
using Common;
using MediatR;

namespace Common.Commands {
    public class GetCommandAsync<TDto> : IRequest<UserModel> {
        public object Bk;

        public GetCommandAsync(object bk) {
            Bk = bk;
        }
    }

}
