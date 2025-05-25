namespace Igloo.Controllers;

using MediatR;
using Microsoft.AspNetCore.Mvc;
using Igloo.UseCases.Users.CreateUser;
using Igloo.UseCases.Users.GetUserById;

[ApiController]
[Route("api/v1/users")]
public class UsersController : ControllerBase
{
    private readonly IMediator _mediator;

    public UsersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserCommand command, CancellationToken cancellationToken)
    {
        var userId = await _mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetUserById), new { id = userId }, new { id = userId });
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetUserById(long id, CancellationToken cancellationToken)
    {
        var user = await _mediator.Send(new GetUserByIdQuery(id), cancellationToken);
        if (user is null)
            return NotFound();
        return Ok(user);
    }
}