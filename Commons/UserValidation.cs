using FluentValidation;

namespace Common {
    public class UserValidation : AbstractValidator<UserModel> {

        public UserValidation() {
            this.Apply();
        }

        private void Apply() {

            RuleFor(user => user.Email)
               .NotEmpty()
               .EmailAddress()
               .NotNull()
               .WithMessage("The Email attribute cannot be blank");

            RuleFor(user => user.Password)
                .NotEmpty()
                .NotNull()
                .WithMessage("The Password attribute cannot be blank");

            RuleFor(user => user.FirstName)
               .NotEmpty()
               .NotNull()
               .WithMessage("The Password attribute cannot be blank");

            RuleFor(user => user.LastName)
              .NotEmpty()
              .NotNull()
              .WithMessage("The Password attribute cannot be blank");
            RuleFor(user => user.Note)
               .NotEmpty()
               .ListMustContainFewerThan(3);
        }
    }
}