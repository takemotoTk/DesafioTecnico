using MediatR;
using VaccinationCardManagement.Application.Constants;
using VaccinationCardManagement.Domain.Adapter;

namespace VaccinationCardManagement.Application.Commands.Person.Delete;

public class DeletePersonCommandHandler: IRequestHandler<DeletePersonCommand>
{
    private readonly IVaccinationCardManagementRepository _repo;
    private readonly ICacheManager _cacheManager;
    public DeletePersonCommandHandler(IVaccinationCardManagementRepository repo, ICacheManager cacheManager)
    {
        _repo = repo;
        _cacheManager = cacheManager;
    }

    public async Task Handle(DeletePersonCommand request, CancellationToken cancellationToken)
    {
        var getPerson = await _repo.FindOne<Domain.Entities.Person>(c => c.Id.Equals(request.IdPerson));
        if (getPerson != null)
        {
            await _repo.DeleteMany<Domain.Entities.VaccinationCard>(c=> c.IdPerson.Equals(request.IdPerson));
            await _repo.Delete(getPerson);

            //clear cache
            await _cacheManager.ClearCacheByKeyAsync(ConstantsCacheKey.GetAllPeople);
        }
    }
}
