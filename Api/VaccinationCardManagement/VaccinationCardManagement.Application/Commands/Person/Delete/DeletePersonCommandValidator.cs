using FluentValidation;

namespace VaccinationCardManagement.Application.Commands.Person.Delete;

public class DeletePersonCommandValidator : AbstractValidator<DeletePersonCommand>
{
    public DeletePersonCommandValidator()
    {
        RuleFor(x => x.IdPerson)
         .NotEmpty()
         .NotNull();
    }
}
