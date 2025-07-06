using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;
using Igloo.Infrastructure.Services;

namespace Igloo.Presentation.Attributes;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class RequireProfileAccessAttribute : Attribute, IAsyncAuthorizationFilter
{
    private readonly string _profileIdParameter;
    private readonly bool _requireActiveProfile;

    public RequireProfileAccessAttribute(string profileIdParameter = "profileId", bool requireActiveProfile = false)
    {
        _profileIdParameter = profileIdParameter;
        _requireActiveProfile = requireActiveProfile;
    }

    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        var user = context.HttpContext.User;
        
        if (!user.Identity?.IsAuthenticated ?? true)
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim == null || !long.TryParse(userIdClaim.Value, out var userId))
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        var profileContextService = context.HttpContext.RequestServices.GetService<IProfileContextService>();
        if (profileContextService == null)
        {
            context.Result = new StatusCodeResult(500);
            return;
        }

        long? profileId = null;

        if (_requireActiveProfile)
        {
            profileId = await profileContextService.GetActiveProfileIdAsync(user);
            if (profileId == null)
            {
                context.Result = new BadRequestObjectResult("No active profile found");
                return;
            }
        }
        else
        {
            if (context.RouteData.Values.TryGetValue(_profileIdParameter, out var profileIdValue))
            {
                if (long.TryParse(profileIdValue?.ToString(), out var routeProfileId))
                {
                    profileId = routeProfileId;
                }
            }
            
            if (profileId == null)
            {
                var queryProfileId = context.HttpContext.Request.Query[_profileIdParameter].FirstOrDefault();
                if (queryProfileId != null && long.TryParse(queryProfileId, out var queryParsedProfileId))
                {
                    profileId = queryParsedProfileId;
                }
            }
        }

        if (profileId == null)
        {
            context.Result = new BadRequestObjectResult($"Parameter '{_profileIdParameter}' is required");
            return;
        }

        var hasAccess = await profileContextService.CanUserAccessProfileAsync(userId, profileId.Value);
        if (!hasAccess)
        {
            context.Result = new ForbidResult("Access denied to the specified profile");
        }
    }
}
