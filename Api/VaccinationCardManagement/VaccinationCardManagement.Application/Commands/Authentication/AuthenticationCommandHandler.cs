using MediatR;
using VaccinationCardManagement.Application.Services.Authentication;
using VaccinationCardManagement.Application.Models;
using VaccinationCardManagement.Application.Services.User;

namespace VaccinationCardManagement.Application.Commands.Authentication;

public class AuthenticationCommandHandler : IRequestHandler<AuthenticationCommand, AuthenticationModel>
{
    private readonly AuthenticationService _authService;
    private readonly UserService _userService;
    public AuthenticationCommandHandler(AuthenticationService authService, UserService userService)
    {
        _authService = authService;
        _userService = userService;
    }

    public async Task<AuthenticationModel> Handle(AuthenticationCommand request, CancellationToken cancellationToken)
    {
        var checkUserValid = await _userService.CheckUserValidAsync(request.Email, request.Password);
        if (!checkUserValid.Item2) {
            throw new Exception("User invalid");
        }

        var user = checkUserValid.Item1;
        return _authService.GenerateToken(user.Id, user.Name);
    }
}
