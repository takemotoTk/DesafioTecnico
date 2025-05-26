using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using VaccinationCardManagement.Application.Commands.Person.Add;
using VaccinationCardManagement.Application.Commands.Person.Delete;
using VaccinationCardManagement.Application.Commands.Person.Update;
using VaccinationCardManagement.Application.Models;
using VaccinationCardManagement.Application.Queries.Person.GetAllPeople;
using VaccinationCardManagement.Application.Queries.Person.GetPerson;
using VaccinationCardManagement.Controllers.Base;

namespace VaccinationCardManagement.Controllers;

[Authorize]
public class PersonController : BaseController
{
    [HttpGet("{idPerson}")]
    [ProducesResponseType(typeof(PersonModel), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetPerson(int idPerson)
    {
        return Ok(await this.Mediator.Send(new GetPersonQuery { IdPerson = idPerson }));
    }

    [HttpGet("GetAllPeople")]
    [ProducesResponseType(typeof(IEnumerable<PersonModel>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetAllPeople()
    {
        return Ok(await this.Mediator.Send(new GetAllPeopleQuery()));
    }

    [HttpPost("AddPerson")]
    [ProducesResponseType(typeof(int), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> AddPerson([FromBody] AddPersonCommand request)
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

    [HttpPatch("UpdatePerson")]
    [ProducesResponseType(typeof(PersonModel), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> UpdatePerson(UpdatePersonCommand request)
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

    [HttpDelete("{idPerson}")]
    public async Task<IActionResult> DeletePerson(int idPerson)
    {
        await this.Mediator.Send(new DeletePersonCommand { IdPerson = idPerson });
        return NoContent();
    }
}
