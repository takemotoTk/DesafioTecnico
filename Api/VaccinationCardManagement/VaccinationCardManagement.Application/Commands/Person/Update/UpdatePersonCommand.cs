using MediatR;
using VaccinationCardManagement.Application.Models;

namespace VaccinationCardManagement.Application.Commands.Person.Update;

public class UpdatePersonCommand : IRequest<PersonModel>
{
    public int IdPerson { get; set; }
    public string Name { get; set; }
}