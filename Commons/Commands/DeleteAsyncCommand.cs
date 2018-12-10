using MediatR;

namespace Common.Commands {
    public class DeleteAsyncCommand<TDto>
        : IRequest<bool>

        where TDto : DTO, new() {

        //business key
        public object Bk { get; set; }

        public DeleteAsyncCommand(object bk) {
            Bk = bk;
        }
    }
}