using MediatR;
using VaccinationCardManagement.Application.Models;
namespace VaccinationCardManagement.Application.Queries.Vaccine.GetVaccine;

public class GetVaccineQuery : IRequest<VaccineModel>
{
    public int IdVaccine { get; init; }
}