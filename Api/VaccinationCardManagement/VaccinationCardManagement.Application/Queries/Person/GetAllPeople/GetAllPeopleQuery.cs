using MediatR;
using VaccinationCardManagement.Application.Models;
using VaccinationCardManagement.Application.Queries.Vaccine.GetAllVaccines;

namespace VaccinationCardManagement.Application.Queries.Person.GetAllPeople;

public class GetAllPeopleQuery : IRequest<IEnumerable<PersonModel>>
{
}