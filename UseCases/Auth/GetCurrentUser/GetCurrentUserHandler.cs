namespace Igloo.UseCases.Auth.GetCurrentUser;

using MediatR;
using Microsoft.EntityFrameworkCore;
using Igloo.Infrastructure.Persistence;
using System.Security.Claims;

public class GetCurrentUserHandler : IRequestHandler<GetCurrentUserQuery, CurrentUserResponse?>
{
    private readonly IglooDbContext _db;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public GetCurrentUserHandler(IglooDbContext db, IHttpContextAccessor httpContextAccessor)
    {
        _db = db;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<CurrentUserResponse?> Handle(GetCurrentUserQuery request, CancellationToken cancellationToken)
    {
        var userIdClaim = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier);
        
        if (userIdClaim == null || !long.TryParse(userIdClaim.Value, out var userId))
            return null;

        var user = await _db.Users
            .FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);

        if (user == null)
            return null;

        return new CurrentUserResponse(
            Id: user.Id,
            Email: user.Email,
            CreatedAt: user.CreatedAt
        );
    }
} 