namespace Igloo.Controllers;

using MediatR;
using Microsoft.AspNetCore.Mvc;
using Igloo.Features.Users.CreateUser;
using Igloo.Features.Users.GetUserById;
using Igloo.Features.Users.Dtos;
using Igloo.Presentation.Controllers.Dtos;

[ApiController]
[Route("api/v1/users")]
public class UsersController : ControllerBase
{
    private readonly IMediator _mediator;

    public UsersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Creates a new user
    /// </summary>
    /// <param name="command">User data to be created</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Created user ID</returns>
    /// <response code="201">User created successfully</response>
    /// <response code="400">Invalid input data</response>
    /// <response code="409">Email already in use</response>
    [HttpPost]
    [ProducesResponseType(typeof(IdResponse), 201)]
    [ProducesResponseType(typeof(ValidationErrorResponse), 400)]
    [ProducesResponseType(typeof(ErrorResponse), 409)]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserCommand command, CancellationToken cancellationToken)
    {
        var userId = await _mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetUserById), new { id = userId }, new { id = userId });
    }

    /// <summary>
    /// Gets a user by ID
    /// </summary>
    /// <param name="id">User ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>User data</returns>
    /// <response code="200">User found</response>
    /// <response code="404">User not found</response>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(UserDto), 200)]
    [ProducesResponseType(typeof(ErrorResponse), 404)]
    public async Task<IActionResult> GetUserById(long id, CancellationToken cancellationToken)
    {
        var user = await _mediator.Send(new GetUserByIdQuery(id), cancellationToken);
        if (user is null)
            return NotFound(new ErrorResponse { Message = "User not found" });
        return Ok(user);
    }
}