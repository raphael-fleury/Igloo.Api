namespace Igloo.Features.Auth.GetCurrentUser;

using MediatR;
using Microsoft.EntityFrameworkCore;
using Igloo.Infrastructure.Persistence;
using System.Security.Claims;
using AutoMapper;

public class GetCurrentUserHandler(
    IglooDbContext db,
    IHttpContextAccessor httpContextAccessor,
    IMapper mapper
) : IRequestHandler<GetCurrentUserQuery, CurrentUserResponse?>
{
    public async Task<CurrentUserResponse?> Handle(GetCurrentUserQuery request, CancellationToken cancellationToken)
    {
        var userIdClaim = httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier);
        
        if (userIdClaim == null || !long.TryParse(userIdClaim.Value, out var userId))
            return null;

        var user = await db.Users
            .FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);

        if (user == null)
            return null;

        return mapper.Map<CurrentUserResponse>(user);
    }
}