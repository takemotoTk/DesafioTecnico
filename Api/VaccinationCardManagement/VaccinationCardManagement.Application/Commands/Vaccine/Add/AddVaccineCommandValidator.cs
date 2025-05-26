using FluentValidation;

namespace VaccinationCardManagement.Application.Commands.Vaccine.Add;

public class AddVaccineCommandValidator : AbstractValidator<AddVaccineCommand>
{
    public AddVaccineCommandValidator()
    {
        RuleFor(x => x.Name)
         .NotEmpty()
         .NotNull();

        RuleFor(x => x.RegisterNumber)
         .NotEmpty()
         .NotNull();

        RuleFor(x => x.MaxDose)
        .NotEmpty()
        .NotNull();
    }
}
