using MediatR;
using VaccinationCardManagement.Application.ExtensionMethods;
using VaccinationCardManagement.Application.Models;
using VaccinationCardManagement.Domain.Adapter;
namespace VaccinationCardManagement.Application.Queries.Vaccine.GetVaccine;

public class GetVaccineQueryHandler : IRequestHandler<GetVaccineQuery, VaccineModel>
{
    private readonly IVaccinationCardManagementRepository _repo;
    public GetVaccineQueryHandler(IVaccinationCardManagementRepository repo)
    {
        _repo = repo;
    }

    public async Task<VaccineModel> Handle(GetVaccineQuery request, CancellationToken cancellationToken)
    {
        VaccineModel response = null;
        var result = await _repo.FindOne<Domain.Entities.Vaccine>(c => c.Id.Equals(request.IdVaccine));
        if (result != null)
        {
            response = result.Map<VaccineModel>();
        }
        return response;
    }
}
