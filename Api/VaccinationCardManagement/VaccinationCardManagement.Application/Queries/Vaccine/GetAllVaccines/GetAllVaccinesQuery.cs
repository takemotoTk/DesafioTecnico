using MediatR;
using VaccinationCardManagement.Application.Models;

namespace VaccinationCardManagement.Application.Queries.Vaccine.GetAllVaccines;

public class GetAllVaccinesQuery : IRequest<IEnumerable<VaccineModel>>
{
}