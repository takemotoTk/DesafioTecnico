using MediatR;

namespace VaccinationCardManagement.Application.Commands.Person.Add;

public class AddPersonCommand : IRequest<int>
{
    public string Name { get; init; }
    public long FiscalDocument { get; init; } //RG or CPF only numbers
}