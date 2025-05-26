using MediatR;
using VaccinationCardManagement.Application.Constants;
using VaccinationCardManagement.Application.ExtensionMethods;
using VaccinationCardManagement.Application.Models;
using VaccinationCardManagement.Domain.Adapter;

namespace VaccinationCardManagement.Application.Queries.Person.GetAllPeople;

public class GetAllPeopleQueryHandler : IRequestHandler<GetAllPeopleQuery, IEnumerable<PersonModel>>
{
    private readonly IVaccinationCardManagementRepository _repo;
    private readonly ICacheManager _cacheManager;
    public GetAllPeopleQueryHandler(IVaccinationCardManagementRepository repo, ICacheManager cacheManager)
    {
        _repo = repo;
        _cacheManager = cacheManager;
    }

    public async Task<IEnumerable<PersonModel>> Handle(GetAllPeopleQuery request, CancellationToken cancellationToken)
    {
        IEnumerable<PersonModel> response = null;
        var result = await TryGetAllPeopleCache();
        if (result != null)
        {
            response = result;
        }

        return response;
    }

    private async Task<IEnumerable<PersonModel>> TryGetAllPeopleCache()
    {
        string keyCache = ConstantsCacheKey.GetAllPeople;
        var resultCache = await _cacheManager.GetFromCacheAsync<IEnumerable<PersonModel>>(keyCache);
        if (resultCache == null)
        {
            var result = await _repo.Find<Domain.Entities.Person>();
            if (result != null)
            {
                var mapObj = result.Map<List<PersonModel>>();
                resultCache = mapObj;
                await _cacheManager.SetCacheAsync(keyCache, mapObj, TimeSpan.FromMinutes(60));
            }
        }

        return resultCache;
    }
}
