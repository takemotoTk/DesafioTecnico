using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using VaccinationCardManagement.Application.Commands.VaccinationCard.Add;
using VaccinationCardManagement.Application.Commands.VaccinationCard.Delete;
using VaccinationCardManagement.Application.Models;
using VaccinationCardManagement.Application.Models.VaccinationCard;
using VaccinationCardManagement.Application.Queries.VaccinationCard.GetVaccinationCardByIdPerson;
using VaccinationCardManagement.Controllers.Base;

namespace VaccinationCardManagement.Controllers;

[Authorize]
public class VaccinationCardController : BaseController
{
    [HttpGet("{idPerson}")]
    [ProducesResponseType(typeof(VaccinationCardByPersonModel), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetVaccinationCardByIdPerson(int idPerson)
    {
        return Ok(await this.Mediator.Send(new GetVaccinationCardByIdPersonQuery { IdPerson = idPerson }));
    }

    [HttpPost("AddVaccination")]
    public async Task<IActionResult> AddVaccination([FromBody] AddVaccinationCardCommand request)
    {
        try
        {
            await this.Mediator.Send(request);
            return NoContent();
        }
        catch (Exception err)
        {
            return BadRequest(err.Message);
        }
    }

    [HttpDelete("{idVaccination}")]
    public async Task<IActionResult> DeleteVaccination(int idVaccination)
    {
        await this.Mediator.Send(new DeleteVaccinationCommand { IdVaccination = idVaccination });
        return NoContent();
    }
}
