using MediatR;
using VaccinationCardManagement.Application.Models.VaccinationCard;

namespace VaccinationCardManagement.Application.Queries.VaccinationCard.GetVaccinationCardByIdPerson;

public class GetVaccinationCardByIdPersonQuery : IRequest<VaccinationCardByPersonModel>
{
    public int IdPerson {  get; init; }
}