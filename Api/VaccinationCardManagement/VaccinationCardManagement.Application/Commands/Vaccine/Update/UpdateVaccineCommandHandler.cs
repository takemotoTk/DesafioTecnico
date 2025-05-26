using MediatR;
using VaccinationCardManagement.Application.Constants;
using VaccinationCardManagement.Application.ExtensionMethods;
using VaccinationCardManagement.Application.Models;
using VaccinationCardManagement.Domain.Adapter;

namespace VaccinationCardManagement.Application.Commands.Vaccine.Update;

public class UpdateVaccineCommandHandler : IRequestHandler<UpdateVaccineCommand, VaccineModel>
{
    private readonly IVaccinationCardManagementRepository _repo;
    private readonly ICacheManager _cacheManager;
    public UpdateVaccineCommandHandler(IVaccinationCardManagementRepository repo, ICacheManager cacheManager)
    {
        _repo = repo;
        _cacheManager = cacheManager;
    }

    public async Task<VaccineModel> Handle(UpdateVaccineCommand request, CancellationToken cancellationToken)
    {
        VaccineModel response = null;
        var result = await GetVaccine(request);

        var vaccineUpd = new Domain.Entities.Vaccine
        {
            Id = result.Id,
            Name = string.IsNullOrEmpty(request.Name) ? result.Name : request.Name,
            MaxDose = request.MaxDose > 0 ? request.MaxDose : result.MaxDose,
            MaxReinforcement = request.MaxReinforcement > 0 ? request.MaxReinforcement : result.MaxReinforcement,
            RegisterNumber = result.RegisterNumber,
            CreatedUTC = result.CreatedUTC
        };
        var vaccine = await _repo.Update(vaccineUpd);
        response = vaccine.Map<VaccineModel>();

        //clear cache
        await _cacheManager.ClearCacheByKeyAsync(ConstantsCacheKey.GetAllVaccines);
        return response;
    }

    private async Task<Domain.Entities.Vaccine> GetVaccine(UpdateVaccineCommand request)
    {
        var result = await _repo.FindOne<Domain.Entities.Vaccine>(c => c.Id.Equals(request.IdVaccine));
        if (result == null)
        {
            throw new Exception("This person don't exists");
        }
        return result;
    }
}