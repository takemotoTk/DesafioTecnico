using VaccinationCardManagement.Domain.Entities.Base;

namespace VaccinationCardManagement.Domain.Entities;

public class Person : EntityBase
{
    public string Name { get; init; }
    public long FiscalDocument { get; init; }
}