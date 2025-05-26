using MediatR;
using VaccinationCardManagement.Application.Constants;
using VaccinationCardManagement.Application.ExtensionMethods;
using VaccinationCardManagement.Domain.Adapter;

namespace VaccinationCardManagement.Application.Commands.Vaccine.Add;

public class AddVaccineCommandHandler : IRequestHandler<AddVaccineCommand, int>
{
    private readonly IVaccinationCardManagementRepository _repo;
    private readonly ICacheManager _cacheManager;

    public AddVaccineCommandHandler(IVaccinationCardManagementRepository repo, ICacheManager cacheManager)
    {
        _repo = repo;
        _cacheManager = cacheManager;
    }

    public async Task<int> Handle(AddVaccineCommand request, CancellationToken cancellationToken)
    {
        var getPerson = await _repo.FindOne<Domain.Entities.Vaccine>(c => c.RegisterNumber.Equals(request.RegisterNumber));
        if (getPerson != null)
        {
            throw new Exception("This vaccine already exists");
        }
        var result = await _repo.Add(request.Map<Domain.Entities.Vaccine>());

        //clear cache
        await _cacheManager.ClearCacheByKeyAsync(ConstantsCacheKey.GetAllVaccines);

        return result.Id;
    }
}