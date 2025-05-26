using MediatR;
namespace VaccinationCardManagement.Application.Commands.User;

public class CreateUserCommand : IRequest<int>
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
}