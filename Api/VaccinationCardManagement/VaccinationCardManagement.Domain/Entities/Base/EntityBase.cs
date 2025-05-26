namespace VaccinationCardManagement.Domain.Entities.Base;

public abstract class EntityBase
{
    public int Id { get; init; }
    public DateTime CreatedUTC { get; init; } = DateTime.UtcNow;
}
