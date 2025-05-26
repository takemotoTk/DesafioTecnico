using MediatR;
using VaccinationCardManagement.Application.ExtensionMethods;
using VaccinationCardManagement.Application.Models;
using VaccinationCardManagement.Domain.Adapter;

namespace VaccinationCardManagement.Application.Queries.Person.GetPerson;

public class GetPersonQueryHandler : IRequestHandler<GetPersonQuery, PersonModel>
{
    private readonly IVaccinationCardManagementRepository _repo;
    public GetPersonQueryHandler(IVaccinationCardManagementRepository repo)
    {
        _repo = repo;
    }

    public async Task<PersonModel> Handle(GetPersonQuery request, CancellationToken cancellationToken)
    {
        PersonModel response = null;
        var result = await _repo.FindOne<Domain.Entities.Person>(c => c.Id.Equals(request.IdPerson));
        if (result != null)
        {
            response = result.Map<PersonModel>();
        }
        return response;
    }
}
