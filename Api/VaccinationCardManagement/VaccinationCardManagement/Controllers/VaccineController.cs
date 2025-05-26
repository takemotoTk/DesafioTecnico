using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using VaccinationCardManagement.Application.Commands.Vaccine.Add;
using VaccinationCardManagement.Application.Commands.Vaccine.Delete;
using VaccinationCardManagement.Application.Commands.Vaccine.Update;
using VaccinationCardManagement.Application.Models;
using VaccinationCardManagement.Application.Queries.Vaccine.GetAllVaccines;
using VaccinationCardManagement.Application.Queries.Vaccine.GetVaccine;
using VaccinationCardManagement.Controllers.Base;

namespace VaccinationCardManagement.Controllers;

[Authorize]
public class VaccineController : BaseController
{
    [HttpGet("{idVaccine}")]
    [ProducesResponseType(typeof(VaccineModel), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetVaccine(int idVaccine)
    {
        return Ok(await this.Mediator.Send(new GetVaccineQuery { IdVaccine = idVaccine }));
    }

    [HttpGet("GetAllVaccines")]
    [ProducesResponseType(typeof(IEnumerable<VaccineModel>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetAllVaccines()
    {
        return Ok(await this.Mediator.Send(new GetAllVaccinesQuery()));
    }

    [HttpPost("AddVaccine")]
    [ProducesResponseType(typeof(int), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> AddVaccine([FromBody] AddVaccineCommand request)
    {
        try
        {
            return Ok(await this.Mediator.Send(request));
        }
        catch (Exception err)
        {
            return BadRequest(err.Message);
        }
    }

    [HttpPatch("UpdateVaccine")]
    [ProducesResponseType(typeof(VaccineModel), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> UpdateVaccine(UpdateVaccineCommand request)
    {
        try
        {
            return Ok(await this.Mediator.Send(request));
        }
        catch (Exception err)
        {
            return BadRequest(err.Message);
        }
    }

    [HttpDelete("{idVaccine}")]
    public async Task<IActionResult> DeleteVaccine(int idVaccine)
    {
        await this.Mediator.Send(new DeleteVaccineCommand { IdVaccine = idVaccine });
        return NoContent();
    }
}
