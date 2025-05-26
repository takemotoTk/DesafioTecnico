using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using VaccinationCardManagement.Application.Commands.Authentication;
using VaccinationCardManagement.Application.Commands.User;
using VaccinationCardManagement.Application.Models;
using VaccinationCardManagement.Application.Services.Authentication;
using VaccinationCardManagement.Controllers.Base;

namespace VaccinationCardManagement.Controllers;

public class AuthenticationController : BaseController
{
    [AllowAnonymous]
    [HttpPost]
    [ProducesResponseType(typeof(AuthenticationModel), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> Authentication([FromBody] AuthenticationCommand request)
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

    [AllowAnonymous]
    [HttpPost("CreateUser")]
    [ProducesResponseType(typeof(int), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserCommand request)
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

    [Authorize]
    [HttpGet("GetUserLogged")]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetUserLogged()
    {
        var userLogged = base.HttpContext.User?.Claims.First(c => c.Type == Constants.ClaimKeyIdentifier)?.Value ?? null;
        return Ok(userLogged);
    }

}
