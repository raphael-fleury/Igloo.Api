namespace Igloo.Controllers;

using MediatR;
using Microsoft.AspNetCore.Mvc;
using Igloo.UseCases.Users.CreateUser;

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
        return Ok();
    }
}