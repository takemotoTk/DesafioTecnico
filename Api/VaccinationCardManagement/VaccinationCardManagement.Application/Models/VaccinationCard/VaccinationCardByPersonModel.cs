namespace VaccinationCardManagement.Application.Models.VaccinationCard;

public class VaccinationCardByPersonModel
{
    public int IdPerson { get; init; }
    public string Name { get; init; }
    public List<VaccinationModel> Vaccines { get; init; }
}