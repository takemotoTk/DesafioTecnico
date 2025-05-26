using MediatR;
using VaccinationCardManagement.Application.Models;

namespace VaccinationCardManagement.Application.Commands.Vaccine.Update;

public class UpdateVaccineCommand : IRequest<VaccineModel>
{
    public int IdVaccine { get; init; }
    public string Name { get; init; }
    public int MaxDose { get; init; }
    public int MaxReinforcement { get; init; }
}