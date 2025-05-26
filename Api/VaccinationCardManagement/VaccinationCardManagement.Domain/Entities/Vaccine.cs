using VaccinationCardManagement.Domain.Entities.Base;

namespace VaccinationCardManagement.Domain.Entities;

public class Vaccine : EntityBase
{
    public string Name { get; init; }
    public int MaxDose { get; init; }
    public int MaxReinforcement { get; init; }
    public Guid RegisterNumber { get; init; }
}