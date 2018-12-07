using System.Threading;
using System.Threading.Tasks;
using Common;
using MediatR;

namespace Common.Commands {
    public class GetUserCommandAsync : IRequest<UserModel> {
        public string Email;

        public GetUserCommandAsync(string email) {
            Email = email;
        }
    }

}
