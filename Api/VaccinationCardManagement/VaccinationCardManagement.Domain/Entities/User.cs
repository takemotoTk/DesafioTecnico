using VaccinationCardManagement.Domain.Entities.Base;

namespace VaccinationCardManagement.Domain.Entities;

public class User : EntityBase
{
    public string Name { get; init; }
    public string Email { get; init; }
    public string Password { get; init; }
}
