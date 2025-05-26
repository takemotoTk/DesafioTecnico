using MediatR;
using VaccinationCardManagement.Application.Constants;
using VaccinationCardManagement.Application.ExtensionMethods;
using VaccinationCardManagement.Application.Models;
using VaccinationCardManagement.Cache.Cache;
using VaccinationCardManagement.Domain.Adapter;

namespace VaccinationCardManagement.Application.Queries.Vaccine.GetAllVaccines;

public class GetAllVaccinesQueryHandler : IRequestHandler<GetAllVaccinesQuery, IEnumerable<VaccineModel>>
{
    private readonly IVaccinationCardManagementRepository _repo;
    private readonly ICacheManager _cacheManager;

    public GetAllVaccinesQueryHandler(IVaccinationCardManagementRepository repo, ICacheManager cacheManager)
    {
        _repo = repo;
        _cacheManager = cacheManager;
    }

    public async Task<IEnumerable<VaccineModel>> Handle(GetAllVaccinesQuery request, CancellationToken cancellationToken)
    {
        IEnumerable<VaccineModel> response = null;
        var result = await TryGetAllVaccinesCache();
        if (result != null) 
        {
            response = result;
        }

        return response;
    }

    private async Task<IEnumerable<VaccineModel>> TryGetAllVaccinesCache()
    {
        string keyCache = ConstantsCacheKey.GetAllVaccines;
        var resultCache = await _cacheManager.GetFromCacheAsync<IEnumerable<VaccineModel>>(keyCache);
        if (resultCache == null)
        {
            var result = await _repo.Find<Domain.Entities.Vaccine>();
            if (result != null)
            {
                var mapObj = result.Map<List<VaccineModel>>();
                resultCache = mapObj;
                await _cacheManager.SetCacheAsync(keyCache, mapObj, TimeSpan.FromMinutes(60));
            }
        }

        return resultCache;
    }
}
