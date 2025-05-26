namespace VaccinationCardManagement.Application.Models;

public record PersonModel
{
    public int Id { get; init; }
    public string Name { get; init; }
    public long FiscalDocument { get; init; }
}