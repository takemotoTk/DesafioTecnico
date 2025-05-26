using VaccinationCardManagement.Domain.Enums;

namespace VaccinationCardManagement.Application.Models.VaccinationCard;

public class VaccinationCardModel
{
    public int Id { get; init; }
    public int IdPerson { get; init; }
    public int IdVaccine { get; init; }
    public AppliedDoseTypeEnum AppliedDoseType { get; init; }
    public string AppliedDoseTypeName { get; init; }
    public DateTime CreatedUTC { get; init; }
}