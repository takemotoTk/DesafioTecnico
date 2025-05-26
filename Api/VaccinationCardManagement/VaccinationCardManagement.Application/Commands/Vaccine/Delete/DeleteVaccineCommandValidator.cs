using FluentValidation;

namespace VaccinationCardManagement.Application.Commands.Vaccine.Delete;

public class DeleteVaccineCommandValidator : AbstractValidator<DeleteVaccineCommand>
{
    public DeleteVaccineCommandValidator()
    {
        RuleFor(x => x.IdVaccine)
         .NotEmpty()
         .NotNull();
    }
}