using FluentValidation;

namespace VaccinationCardManagement.Application.Commands.VaccinationCard.Add;

public class AddVaccinationCardCommandValidator : AbstractValidator<AddVaccinationCardCommand>
{
    public AddVaccinationCardCommandValidator()
    {
        RuleFor(x => x.IdPerson)
         .NotEmpty()
         .NotNull();

        RuleFor(x => x.IdVaccine)
         .NotEmpty()
         .NotNull();

        RuleFor(x => x.AppliedDoseType)
         .NotEmpty()
         .NotNull()
         .IsInEnum();
    }
}
