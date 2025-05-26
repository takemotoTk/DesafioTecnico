using MediatR;

namespace VaccinationCardManagement.Application.Commands.Vaccine.Add;

public class AddVaccineCommand : IRequest<int>
{
    public string Name { get; init; }
    public int MaxDose { get; init; }
    public int MaxReinforcement { get; init; }
    public Guid RegisterNumber { get; init; }
}