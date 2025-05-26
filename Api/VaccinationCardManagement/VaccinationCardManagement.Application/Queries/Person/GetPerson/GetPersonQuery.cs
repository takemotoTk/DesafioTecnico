using MediatR;
using VaccinationCardManagement.Application.Models;

namespace VaccinationCardManagement.Application.Queries.Person.GetPerson;

public class GetPersonQuery : IRequest<PersonModel>
{
    public int IdPerson { get; init; }
}