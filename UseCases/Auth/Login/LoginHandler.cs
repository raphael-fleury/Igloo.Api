namespace Igloo.UseCases.Auth.Login;

using MediatR;
using BCrypt.Net;
using Microsoft.EntityFrameworkCore;
using Igloo.Infrastructure.Persistence;
using Igloo.Infrastructure.Services;
using FluentValidation;

public class LoginHandler : IRequestHandler<LoginCommand, LoginResponse>
{
    private readonly IglooDbContext _db;
    private readonly IValidator<LoginCommand> _validator;
    private readonly IJwtService _jwtService;

    public LoginHandler(IglooDbContext db, IValidator<LoginCommand> validator, IJwtService jwtService)
    {
        _db = db;
        _validator = validator;
        _jwtService = jwtService;
    }

    public async Task<LoginResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        await _validator.ValidateAndThrowAsync(request, cancellationToken);

        var user = await _db.Users
            .Include(u => u.UserProfiles)
            .ThenInclude(up => up.Profile)
            .FirstOrDefaultAsync(u => u.Email == request.Email, cancellationToken);

        if (user == null)
            throw new ArgumentException("Invalid email or password");

        if (!BCrypt.Verify(request.Password, user.PasswordHash))
            throw new ArgumentException("Invalid email or password");

        // Get the first active profile of the user to be the default
        var defaultProfile = user.UserProfiles
            .Where(up => !up.Profile.IsDeactivated)
            .OrderBy(up => up.Profile.CreatedAt)
            .FirstOrDefault();

        var token = _jwtService.GenerateToken(user.Id, user.Email, defaultProfile?.ProfileId);
        var refreshToken = _jwtService.GenerateRefreshToken();
        var expiresAt = DateTime.UtcNow.AddMinutes(60);

        return new LoginResponse(
            Token: token,
            RefreshToken: refreshToken,
            ExpiresAt: expiresAt,
            UserId: user.Id,
            Email: user.Email,
            ActiveProfileId: defaultProfile?.ProfileId
        );
    }
} 