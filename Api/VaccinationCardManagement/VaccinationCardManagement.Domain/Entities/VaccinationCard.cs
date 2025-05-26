using System.ComponentModel.DataAnnotations.Schema;
using VaccinationCardManagement.Domain.Entities.Base;
using VaccinationCardManagement.Domain.Enums;

namespace VaccinationCardManagement.Domain.Entities;

public class VaccinationCard : EntityBase
{
    public int IdPerson { get; init; }
    public int IdVaccine { get; init; }
    public AppliedDoseTypeEnum AppliedDoseType { get; init; }
    public string AppliedDoseTypeName { get; init; }

    public Person Person { get; set; }
    public Vaccine Vaccine { get; set; }
}