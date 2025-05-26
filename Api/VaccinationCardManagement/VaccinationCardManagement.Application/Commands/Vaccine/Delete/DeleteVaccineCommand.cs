using MediatR;

namespace VaccinationCardManagement.Application.Commands.Vaccine.Delete;

public class DeleteVaccineCommand : IRequest
{
    public int IdVaccine { get; init; }
}