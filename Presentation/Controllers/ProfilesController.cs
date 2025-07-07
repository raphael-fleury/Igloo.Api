namespace Igloo.Controllers;

using MediatR;
using Microsoft.AspNetCore.Mvc;
using Igloo.UseCases.Profiles.CreateProfile;
using Igloo.Presentation.Attributes;
using Igloo.UseCases.Profiles.Dtos;
using Igloo.Presentation.Controllers.Dtos;


[ApiController]
[Route("api/v1/profiles")]
[Authorize]
public class ProfilesController : ControllerBase
{
    private readonly IMediator _mediator;

    public ProfilesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Creates a new profile for the authenticated user
    /// </summary>
    /// <param name="command">Profile data to be created</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Created profile ID</returns>
    /// <response code="201">Profile created successfully</response>
    /// <response code="400">Invalid input data</response>
    /// <response code="401">User not authenticated</response>
    /// <response code="409">Username already in use</response>
    [HttpPost]
    [ProducesResponseType(typeof(ProfileDto), 201)]
    [ProducesResponseType(typeof(ValidationErrorResponse), 400)]
    [ProducesResponseType(typeof(ErrorResponse), 401)]
    [ProducesResponseType(typeof(ErrorResponse), 409)]
    public async Task<IActionResult> CreateProfile([FromBody] CreateProfileCommand command, CancellationToken cancellationToken)
    {
        var profileId = await _mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetProfileById), new { id = profileId }, new { id = profileId });
    }

    /// <summary>
    /// Gets a profile by ID
    /// </summary>
    /// <param name="id">Profile ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Profile data</returns>
    /// <response code="200">Profile found</response>
    /// <response code="404">Profile not found</response>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ProfileDto), 200)]
    [ProducesResponseType(typeof(ErrorResponse), 404)]
    public async Task<IActionResult> GetProfileById(long id, CancellationToken cancellationToken)
    {
        // TODO: Implement GetProfileById use case
        return NotFound();
    }
} 