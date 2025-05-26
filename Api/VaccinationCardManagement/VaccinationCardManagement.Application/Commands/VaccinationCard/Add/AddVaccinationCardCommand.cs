using MediatR;
using VaccinationCardManagement.Domain.Enums;

namespace VaccinationCardManagement.Application.Commands.VaccinationCard.Add;

public class AddVaccinationCardCommand : IRequest
{
    public int IdPerson { get; init; }
    public int IdVaccine { get; init; }
    public AppliedDoseTypeEnum AppliedDoseType { get; init; }
}