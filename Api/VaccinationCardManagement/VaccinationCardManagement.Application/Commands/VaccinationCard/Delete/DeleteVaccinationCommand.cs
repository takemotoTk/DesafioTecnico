using MediatR;

namespace VaccinationCardManagement.Application.Commands.VaccinationCard.Delete;

public class DeleteVaccinationCommand : IRequest
{
    public int IdVaccination {  get; init; }
}