using MediatR;

namespace VaccinationCardManagement.Application.Commands.Person.Delete;

public class DeletePersonCommand : IRequest
{
    public int IdPerson { get; set; }
}