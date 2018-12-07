using MediatR;

namespace Common.Commands {
    public class CreateCommand<TDto> :
        IRequest<TDto>
        where TDto : DTO, new() {

        public TDto Model { get; set; }

        public CreateCommand(TDto model) {
            Model = model;
        }
    }
}
