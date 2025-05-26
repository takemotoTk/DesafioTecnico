using FluentValidation;
namespace VaccinationCardManagement.Application.Commands.Person.Add;

public class AddPersonCommandValidator : AbstractValidator<AddPersonCommand>
{
    public AddPersonCommandValidator()
    {
        RuleFor(x => x.Name)
         .NotEmpty()
         .NotNull()
         .MinimumLength(6);

        RuleFor(x => x.FiscalDocument)
         .NotEmpty()
         .NotNull();
    }
}
