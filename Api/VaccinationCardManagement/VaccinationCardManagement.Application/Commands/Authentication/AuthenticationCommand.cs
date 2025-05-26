using MediatR;
using VaccinationCardManagement.Application.Models;

namespace VaccinationCardManagement.Application.Commands.Authentication;

public class AuthenticationCommand : IRequest<AuthenticationModel>
{
    public string Email { get; init; }
    public string Password { get; init; }
}