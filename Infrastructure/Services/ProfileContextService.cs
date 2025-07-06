using System.Security.Claims;
using Igloo.Domain.Entities;
using Igloo.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Igloo.Infrastructure.Services;

public interface IProfileContextService
{
    Task<long?> GetActiveProfileIdAsync(ClaimsPrincipal principal);
    Task<Profile?> GetActiveProfileAsync(ClaimsPrincipal principal);
    Task<List<Profile>> GetUserProfilesAsync(long userId);
    Task<bool> CanUserAccessProfileAsync(long userId, long profileId);
    Task<string> SwitchProfileAsync(long userId, long profileId);
}

public class ProfileContextService : IProfileContextService
{
    private readonly IglooDbContext _context;
    private readonly IJwtService _jwtService;

    public ProfileContextService(IglooDbContext context, IJwtService jwtService)
    {
        _context = context;
        _jwtService = jwtService;
    }

    public async Task<long?> GetActiveProfileIdAsync(ClaimsPrincipal principal)
    {
        var activeProfileId = _jwtService.GetActiveProfileId(principal);
        
        if (activeProfileId.HasValue)
        {
            return activeProfileId.Value;
        }

        var userIdClaim = principal.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim != null && long.TryParse(userIdClaim.Value, out var userId))
        {
            var firstProfile = await _context.UserProfiles
                .Include(up => up.Profile)
                .Where(up => up.UserId == userId && !up.Profile.IsDeactivated)
                .OrderBy(up => up.Profile.CreatedAt)
                .FirstOrDefaultAsync();

            return firstProfile?.ProfileId;
        }

        return null;
    }

    public async Task<Profile?> GetActiveProfileAsync(ClaimsPrincipal principal)
    {
        var activeProfileId = await GetActiveProfileIdAsync(principal);
        
        if (activeProfileId.HasValue)
        {
            return await _context.Profiles
                .FirstOrDefaultAsync(p => p.Id == activeProfileId.Value);
        }

        return null;
    }

    public async Task<List<Profile>> GetUserProfilesAsync(long userId)
    {
        return await _context.UserProfiles
            .Include(up => up.Profile)
            .Where(up => up.UserId == userId && !up.Profile.IsDeactivated)
            .Select(up => up.Profile)
            .ToListAsync();
    }

    public async Task<bool> CanUserAccessProfileAsync(long userId, long profileId)
    {
        return await _context.UserProfiles
            .AnyAsync(up => up.UserId == userId && up.ProfileId == profileId);
    }

    public async Task<string> SwitchProfileAsync(long userId, long profileId)
    {
        if (!await CanUserAccessProfileAsync(userId, profileId))
        {
            throw new UnauthorizedAccessException("User does not have access to this profile");
        }

        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Id == userId);

        if (user == null)
        {
            throw new InvalidOperationException("User not found");
        }

        return _jwtService.GenerateToken(userId, user.Email, profileId);
    }
}
