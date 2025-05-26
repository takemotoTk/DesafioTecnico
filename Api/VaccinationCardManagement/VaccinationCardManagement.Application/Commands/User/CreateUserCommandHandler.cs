using MediatR;
using VaccinationCardManagement.Application.Services.User;

namespace VaccinationCardManagement.Application.Commands.User;

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, int>
{
    private readonly UserService _userService;
    public CreateUserCommandHandler(UserService userService)
    {
        _userService = userService;
    }

    public async Task<int> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        return await _userService.AddUserAsync(request.Name, request.Email, request.Password);
    }
}

