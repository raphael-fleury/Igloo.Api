namespace Igloo.Features.Auth.GetCurrentUser;

using MediatR;
using Microsoft.EntityFrameworkCore;
using Igloo.Infrastructure.Persistence;
using System.Security.Claims;
using AutoMapper;

public class GetCurrentUserHandler : IRequestHandler<GetCurrentUserQuery, CurrentUserResponse?>
{
    private readonly IglooDbContext _db;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IMapper _mapper;

    public GetCurrentUserHandler(IglooDbContext db, IHttpContextAccessor httpContextAccessor, IMapper mapper)
    {
        _db = db;
        _httpContextAccessor = httpContextAccessor;
        _mapper = mapper;
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

        return _mapper.Map<CurrentUserResponse>(user);
    }
} 