using MediatR;
using System.Linq;
using VaccinationCardManagement.Application.Commands.VaccinationCard.Add;
using VaccinationCardManagement.Application.Constants;
using VaccinationCardManagement.Application.ExtensionMethods;
using VaccinationCardManagement.Application.Models;
using VaccinationCardManagement.Application.Models.VaccinationCard;
using VaccinationCardManagement.Application.Queries.Person.GetPerson;
using VaccinationCardManagement.Application.Queries.Vaccine.GetAllVaccines;
using VaccinationCardManagement.Application.Queries.Vaccine.GetVaccine;
using VaccinationCardManagement.Domain.Adapter;
using VaccinationCardManagement.Domain.Entities;
using VaccinationCardManagement.Domain.Enums;

namespace VaccinationCardManagement.Application.Queries.VaccinationCard.GetVaccinationCardByIdPerson;

public class GetVaccinationCardByIdPersonQueryHandler : IRequestHandler<GetVaccinationCardByIdPersonQuery, VaccinationCardByPersonModel>
{
    private readonly IVaccinationCardManagementRepository _repo;
    private readonly ICacheManager _cacheManager;
    private readonly ISender _mediator;
    public GetVaccinationCardByIdPersonQueryHandler(
        ISender mediator, 
        IVaccinationCardManagementRepository repo,
        ICacheManager cacheManager)
    {
        _mediator = mediator;
        _repo = repo;
        _cacheManager = cacheManager;
    }

    public async Task<VaccinationCardByPersonModel> Handle(GetVaccinationCardByIdPersonQuery request, CancellationToken cancellationToken)
    {
        VaccinationCardByPersonModel response = null;
        var person = await GetPerson(request.IdPerson);
        var vaccines = await GetAllVaccines();

        Dictionary<int, IGrouping<int, VaccinationCardModel>> dictVaccinations = null;
        var vaccinations = await TryGetAllVaccinationCardCache(request.IdPerson);
        if (vaccinations != null && vaccinations.Count() > 0)
        {
            dictVaccinations = vaccinations
                .GroupBy(c => c.IdVaccine)
                .ToDictionary(c => c.Key, c => c);
        }

        return response = new VaccinationCardByPersonModel
        {
            IdPerson = person.Id,
            Name = person.Name,
            Vaccines = MakeVaccineListModel(vaccines, dictVaccinations)
        };
    }

    private List<VaccinationModel> MakeVaccineListModel(
        IEnumerable<VaccineModel> vaccines,
        Dictionary<int, IGrouping<int, VaccinationCardModel>> dictVaccinations)
    {
        var result = new List<VaccinationModel>();
        foreach (var vaccine in vaccines)
        {
            Dictionary<AppliedDoseTypeEnum, VaccinationCardModel> appliedDoses = new Dictionary<AppliedDoseTypeEnum, VaccinationCardModel>();

            // Pego o dicionário de doses aplicadas(por tipo)
            if (dictVaccinations != null)
            {
                appliedDoses = dictVaccinations.TryGetValue(vaccine.Id, out var appliedGroup)
                                ? appliedGroup.GroupBy(c => c.AppliedDoseType)
                                              .ToDictionary(g => g.Key, g => g.FirstOrDefault())
                                : new Dictionary<AppliedDoseTypeEnum, VaccinationCardModel>();
            }

            // Função local para criar um VaccinationDetailsModel
            VaccinationDetailsModel GetDose(AppliedDoseTypeEnum doseType, bool hasDose) =>
                hasDose
                    ? new VaccinationDetailsModel
                    {
                        AppliedDoseDateTime = appliedDoses.TryGetValue(doseType, out var dose) ? dose?.CreatedUTC : null,
                        IdVaccinationCard = appliedDoses.TryGetValue(doseType, out dose) ? dose?.Id : null,
                        Situation = appliedDoses.ContainsKey(doseType) ? VaccineSituationEnum.Applied : VaccineSituationEnum.NotApplied
                    }
                    : null;

            var model = new VaccinationModel
            {
                Id = vaccine.Id,
                VaccineName = vaccine.Name,
                Dose1 = GetDose(AppliedDoseTypeEnum.Dose1, vaccine.MaxDose >= 1),
                Dose2 = GetDose(AppliedDoseTypeEnum.Dose2, vaccine.MaxDose >= 2),
                Dose3 = GetDose(AppliedDoseTypeEnum.Dose3, vaccine.MaxDose >= 3),
                Reinforcement1 = GetDose(AppliedDoseTypeEnum.Reinforcement1, vaccine.MaxReinforcement >= 1),
                Reinforcement2 = GetDose(AppliedDoseTypeEnum.Reinforcement2, vaccine.MaxReinforcement >= 2)
            };

            result.Add(model);
        }

        return result;
    }

    private async Task<Models.PersonModel> GetPerson(int idPerson)
    {
        var person = await _mediator.Send(new GetPersonQuery { IdPerson = idPerson });
        if (person == null)
        {
            throw new Exception("This person don't exists");
        }
        return person;
    }

    private async Task<IEnumerable<Models.VaccineModel>> GetAllVaccines()
    {
        var vaccines = await _mediator.Send(new GetAllVaccinesQuery());
        if (vaccines == null)
        {
            throw new Exception("Add someone vaccine");
        }
        return vaccines;
    }

    private async Task<IEnumerable<Models.VaccinationCard.VaccinationCardModel>> TryGetAllVaccinationCardCache(int idPerson)
    {
        string keyCache = $"{ConstantsCacheKey.GetAllVaccinationCardByPerson}-{idPerson}";
        var resultCache = await _cacheManager.GetFromCacheAsync<IEnumerable<Models.VaccinationCard.VaccinationCardModel>>(keyCache);
        if (resultCache == null)
        {
            var result = await _repo.Find<Domain.Entities.VaccinationCard>(c=> c.IdPerson.Equals(idPerson));
            if (result != null)
            {
                var mapObj = result.Map<List<Models.VaccinationCard.VaccinationCardModel>>();
                resultCache = mapObj;
                await _cacheManager.SetCacheAsync(keyCache, mapObj, TimeSpan.FromMinutes(60));
            }
        }

        return resultCache;
    }
}
