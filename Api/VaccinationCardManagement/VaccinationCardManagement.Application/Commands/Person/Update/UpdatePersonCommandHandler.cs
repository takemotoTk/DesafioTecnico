using MediatR;
using VaccinationCardManagement.Application.Constants;
using VaccinationCardManagement.Application.ExtensionMethods;
using VaccinationCardManagement.Application.Models;
using VaccinationCardManagement.Cache.Cache;
using VaccinationCardManagement.Domain.Adapter;

namespace VaccinationCardManagement.Application.Commands.Person.Update;

public class UpdatePersonCommandHandler : IRequestHandler<UpdatePersonCommand, PersonModel>
{
    private readonly IVaccinationCardManagementRepository _repo;
    private readonly ICacheManager _cacheManager;
    public UpdatePersonCommandHandler(IVaccinationCardManagementRepository repo, ICacheManager cacheManager)
    {
        _repo = repo;
        _cacheManager = cacheManager;
    }

    public async Task<PersonModel> Handle(UpdatePersonCommand request, CancellationToken cancellationToken)
    {
        PersonModel response = null;
        var result = await GetPerson(request);

        var personUpd = new Domain.Entities.Person
        {
            Id = result.Id,
            Name = string.IsNullOrEmpty(request.Name) ? result.Name : request.Name,
            FiscalDocument = result.FiscalDocument,
            CreatedUTC = result.CreatedUTC
        };
        var person = await _repo.Update(personUpd);
        response = person.Map<PersonModel>();

        //clear cache
        await _cacheManager.ClearCacheByKeyAsync(ConstantsCacheKey.GetAllPeople);

        return response;
    }

    private async Task<Domain.Entities.Person> GetPerson(UpdatePersonCommand request)
    {
        var result = await _repo.FindOne<Domain.Entities.Person>(c => c.Id.Equals(request.IdPerson));
        if (result == null)
        {
            throw new Exception("This person don't exists");
        }

        return result;
    }
}
