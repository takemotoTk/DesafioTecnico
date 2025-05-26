using VaccinationCardManagement.Domain.Enums;

namespace VaccinationCardManagement.Application.Models.VaccinationCard;

public class VaccinationDetailsModel
{
    public int? IdVaccinationCard { get; init; } = null;
    public DateTime? AppliedDoseDateTime { get; init; } = null;
    public VaccineSituationEnum Situation { get; init; } = VaccineSituationEnum.NotApplied;
}