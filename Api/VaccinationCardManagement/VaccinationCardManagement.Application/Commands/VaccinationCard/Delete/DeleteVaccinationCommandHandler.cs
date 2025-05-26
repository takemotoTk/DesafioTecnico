using MediatR;
using VaccinationCardManagement.Application.Constants;
using VaccinationCardManagement.Cache.Cache;
using VaccinationCardManagement.Domain.Adapter;

namespace VaccinationCardManagement.Application.Commands.VaccinationCard.Delete;

public class DeleteVaccinationCommandHandler : IRequestHandler<DeleteVaccinationCommand>
{
    private readonly IVaccinationCardManagementRepository _repo;
    private readonly ICacheManager _cacheManager;
    public DeleteVaccinationCommandHandler(IVaccinationCardManagementRepository repo, ICacheManager cacheManager)
    {
        _repo = repo;
        _cacheManager = cacheManager;
    }

    public async Task Handle(DeleteVaccinationCommand request, CancellationToken cancellationToken)
    {
        var result = await _repo.FindOne<Domain.Entities.VaccinationCard>(c => c.Id.Equals(request.IdVaccination));
        if (result != null)
        {
            await _repo.Delete(result);

            //clear cache
            await _cacheManager.ClearCacheByKeyAsync($"{ConstantsCacheKey.GetAllVaccinationCardByPerson}-{result.IdPerson}");
        }
    }
}
