using MediatR;
using VaccinationCardManagement.Application.Constants;
using VaccinationCardManagement.Application.Models;
using VaccinationCardManagement.Application.Queries.Person.GetPerson;
using VaccinationCardManagement.Application.Queries.Vaccine.GetVaccine;
using VaccinationCardManagement.Cache.Cache;
using VaccinationCardManagement.Domain.Adapter;
using VaccinationCardManagement.Domain.Entities;
using VaccinationCardManagement.Domain.Enums;

namespace VaccinationCardManagement.Application.Commands.VaccinationCard.Add;

public class AddVaccinationCardCommandHandler : IRequestHandler<AddVaccinationCardCommand>
{
    private readonly IVaccinationCardManagementRepository _repo;
    private readonly ICacheManager _cacheManager;
    private readonly ISender _mediator;

    public AddVaccinationCardCommandHandler(ISender mediator, IVaccinationCardManagementRepository repo, ICacheManager cacheManager)
    {
        _mediator = mediator;
        _repo = repo;
        _cacheManager = cacheManager;
    }

    public async Task Handle(AddVaccinationCardCommand request, CancellationToken cancellationToken)
    {
        await GetPerson(request);
        var vaccine = await GetVaccine(request);

        await CheckAppliedDose(request, vaccine);

        var appliedDoseTypeName = request.AppliedDoseType switch
        {
            AppliedDoseTypeEnum.Dose1 => "1ª Dose",
            AppliedDoseTypeEnum.Dose2 => "2ª Dose",
            AppliedDoseTypeEnum.Dose3 => "3ª Dose",
            AppliedDoseTypeEnum.Reinforcement1 => "1º Reforço",
            AppliedDoseTypeEnum.Reinforcement2 => "2º Reforço",
            _ => throw new ArgumentOutOfRangeException(nameof(AppliedDoseTypeEnum), "Invalid dose type.")
        };

        var vaccinationCard = new Domain.Entities.VaccinationCard
        {
            IdPerson = request.IdPerson,
            IdVaccine = request.IdVaccine,
            AppliedDoseType = request.AppliedDoseType,
            AppliedDoseTypeName = appliedDoseTypeName,
        };
        await _repo.Add(vaccinationCard);

        //clear cache
        await _cacheManager.ClearCacheByKeyAsync($"{ConstantsCacheKey.GetAllVaccinationCardByPerson}-{request.IdPerson}");
    }

    private async Task<Models.VaccineModel> GetVaccine(AddVaccinationCardCommand request)
    {
        var vaccine = await _mediator.Send(new GetVaccineQuery { IdVaccine = request.IdVaccine });
        if (vaccine == null)
        {
            throw new Exception("This vaccine don't exists");
        }
        return vaccine;
    }

    private async Task GetPerson(AddVaccinationCardCommand request)
    {
        var person = await _mediator.Send(new GetPersonQuery { IdPerson = request.IdPerson });
        if (person == null)
        {
            throw new Exception("This person don't exists");
        }
    }

    private async Task<bool> CheckAppliedDose(AddVaccinationCardCommand request, VaccineModel vaccine)
    {
        var existingDoses = (await _repo.Find<Domain.Entities.VaccinationCard>(c =>
            c.IdPerson == request.IdPerson &&
            c.IdVaccine == request.IdVaccine))
            .Select(c => c.AppliedDoseType)
            .ToHashSet();

        bool HasDoses(params AppliedDoseTypeEnum[] doses) => doses.All(existingDoses.Contains);

        bool isValid = HasDoses(request.AppliedDoseType) ? false : true; //avoid duplication
        if (isValid)
        {
            //check vaccines based on requested doses
            isValid = request.AppliedDoseType switch
            {
                AppliedDoseTypeEnum.Dose1 => !existingDoses.Any(),
                AppliedDoseTypeEnum.Dose2 => HasDoses(AppliedDoseTypeEnum.Dose1) && vaccine.MaxDose >= 2,
                AppliedDoseTypeEnum.Dose3 => HasDoses(AppliedDoseTypeEnum.Dose1, AppliedDoseTypeEnum.Dose2) && vaccine.MaxDose >= 3,
                AppliedDoseTypeEnum.Reinforcement1 => vaccine.MaxReinforcement >= 1 && HasRequiredDosesForReinforcement(vaccine, existingDoses),
                AppliedDoseTypeEnum.Reinforcement2 => vaccine.MaxReinforcement >= 2 && HasRequiredDosesForReinforcement(vaccine, existingDoses),
                _ => false
            };
        }

        if (!isValid)
        {
            throw new Exception("This dose cannot be applied");
        }

        return true;
    }

    private static bool HasRequiredDosesForReinforcement(VaccineModel vaccine, HashSet<AppliedDoseTypeEnum> existingDoses)
    {
        var requiredDoses = new List<AppliedDoseTypeEnum>();

        if (vaccine.MaxDose >= 1) requiredDoses.Add(AppliedDoseTypeEnum.Dose1);
        if (vaccine.MaxDose >= 2) requiredDoses.Add(AppliedDoseTypeEnum.Dose2);
        if (vaccine.MaxDose >= 3) requiredDoses.Add(AppliedDoseTypeEnum.Dose3);

        return requiredDoses.All(existingDoses.Contains);
    }
}