using MediatR;

namespace Common.Commands {

    public interface ICreateCommand<out T>
        : IRequest<T> where T : DTO, new() {
    }
}
