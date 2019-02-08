using Common.Info;
using FluentValidation;
using MediatR;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Common.Commands {
    public class AddUserWithNotesAsyncCommand : IRequest<UserModel> {
        public UserModel userModel;
        public AddUserWithNotesAsyncCommand(UserModel user) {
            userModel = user;
        }
    }
    //public class AsyncValidationPipeline<TRequest, TResponse>
    //    : IPipelineBehavior<TRequest, TResponse>
    //    where TRequest : IRequest<TResponse> {

    //    private readonly IEnumerable<IValidator<TRequest>> _validators;

    //    public AsyncValidationPipeline(
    //        IEnumerable<IValidator<TRequest>> validators) {

    //        _validators = validators;
    //    }

    //    public Task<TResponse> Handle(TRequest request,
    //        CancellationToken cancellationToken,
    //        RequestHandlerDelegate<TResponse> next) {

    //        var context = new ValidationContext(request);
    //        var failures = _validators
    //            .Select(v => v.Validate(context))
    //            .SelectMany(result => result.Errors)
    //            .Where(f => f != null)
    //            .ToList();

    //        if (failures.Count != 0) {
    //            throw new ValidationException(failures);
    //        }
    //        return next();
    //    }
    //}
}
