using MediatR;
using VaccinationCardManagement.Application.Constants;
using VaccinationCardManagement.Application.ExtensionMethods;
using VaccinationCardManagement.Cache.Cache;
using VaccinationCardManagement.Domain.Adapter;

namespace VaccinationCardManagement.Application.Commands.Person.Add;

public class AddPersonCommandHandler : IRequestHandler<AddPersonCommand, int>
{
    private readonly IVaccinationCardManagementRepository _repo;
    private readonly ICacheManager _cacheManager;
    public AddPersonCommandHandler(IVaccinationCardManagementRepository repo, ICacheManager cacheManager)
    {
        _repo = repo;
        _cacheManager = cacheManager;
    }

    public async Task<int> Handle(AddPersonCommand request, CancellationToken cancellationToken)
    {
        var getPerson = await _repo.FindOne<Domain.Entities.Person>(c => c.FiscalDocument.Equals(request.FiscalDocument));
        if (getPerson != null) 
        {
            throw new Exception("This person already exists");
        }
        var result = await _repo.Add(request.Map<Domain.Entities.Person>());

        //clear cache
        await _cacheManager.ClearCacheByKeyAsync(ConstantsCacheKey.GetAllPeople);

        return result.Id;
    }
}


