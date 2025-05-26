namespace VaccinationCardManagement.Application.Models;

public record VaccineModel
{
    public int Id { get; init; }
    public string Name { get; init; }
    public int MaxDose { get; init; }
    public int MaxReinforcement { get; init; }
    public Guid RegisterNumber { get; init; }
}