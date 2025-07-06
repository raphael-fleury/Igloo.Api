namespace Igloo.UseCases.Auth.Login;

using FluentValidation;

public class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    public LoginCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress()
            .WithMessage("Invalid email");

        RuleFor(x => x.Password)
            .NotEmpty()
            .WithMessage("Password is required");
    }
} 