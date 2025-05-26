namespace VaccinationCardManagement.Application.Models.VaccinationCard;

public class VaccinationModel
{
    public int Id { get; init; }
    public string VaccineName { get; init; }
    public VaccinationDetailsModel Dose1 { get; init; }
    public VaccinationDetailsModel? Dose2 { get; init; }
    public VaccinationDetailsModel? Dose3 { get; init; }
    public VaccinationDetailsModel? Reinforcement1 { get; init; }
    public VaccinationDetailsModel? Reinforcement2 { get; init; }
}
