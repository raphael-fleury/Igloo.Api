namespace Igloo.UseCases.Profiles.CreateProfile;

using FluentValidation;

public class CreateProfileCommandValidator : AbstractValidator<CreateProfileCommand>
{
    public CreateProfileCommandValidator()
    {
        RuleFor(x => x.Username)
            .NotEmpty()
            .MinimumLength(3)
            .MaximumLength(24)
            .Matches("^[a-zA-Z0-9_.]+$")
            .WithMessage("Username must contain only letters, numbers, dot and underscore");

        RuleFor(x => x.DisplayName)
            .NotEmpty()
            .MinimumLength(1)
            .MaximumLength(50);

        RuleFor(x => x.Bio)
            .MaximumLength(160)
            .When(x => !string.IsNullOrEmpty(x.Bio));
    }
} 