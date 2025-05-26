using FluentValidation;

namespace VaccinationCardManagement.Application.Commands.User;

public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
    public CreateUserCommandValidator()
    {
        RuleFor(x => x.Name)
         .NotEmpty()
         .NotNull();

        RuleFor(x => x.Email)
         .NotEmpty()
         .NotNull()
         .EmailAddress();

        RuleFor(x => x.Password)
         .NotEmpty()
         .NotNull()
         .MinimumLength(6);
    }
}
