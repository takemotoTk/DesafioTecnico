using MediatR;
using VaccinationCardManagement.Application.Constants;
using VaccinationCardManagement.Domain.Adapter;

namespace VaccinationCardManagement.Application.Commands.Vaccine.Delete;

public class DeleteVaccineCommandHandler : IRequestHandler<DeleteVaccineCommand>
{
    private readonly IVaccinationCardManagementRepository _repo;
    private readonly ICacheManager _cacheManager;

    public DeleteVaccineCommandHandler(IVaccinationCardManagementRepository repo, ICacheManager cacheManager)
    {
        _repo = repo;
        _cacheManager = cacheManager;
    }

    public async Task Handle(DeleteVaccineCommand request, CancellationToken cancellationToken)
    {
        var getVaccine = await _repo.FindOne<Domain.Entities.Vaccine>(c => c.Id.Equals(request.IdVaccine));
        if (getVaccine != null)
        {
            await _repo.Delete(getVaccine);

            //clear cache
            await _cacheManager.ClearCacheByKeyAsync(ConstantsCacheKey.GetAllVaccines);
        }
    }
}