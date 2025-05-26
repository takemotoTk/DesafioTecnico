using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VaccinationCardManagement.Application.Commands.Authentication;

public class AuthenticationCommandValidator : AbstractValidator<AuthenticationCommand>
{
    public AuthenticationCommandValidator()
    {
        RuleFor(x => x.Email)
         .NotEmpty()
         .NotNull()
         .EmailAddress();

        RuleFor(x => x.Password)
         .NotEmpty()
         .NotNull()
         .MinimumLength(6);
    }
}
