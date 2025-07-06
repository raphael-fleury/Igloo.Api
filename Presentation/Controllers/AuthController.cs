namespace Igloo.Controllers;

using MediatR;
using Microsoft.AspNetCore.Mvc;
using Igloo.UseCases.Auth.Login;
using Igloo.UseCases.Auth.GetCurrentUser;
using Igloo.Presentation.Attributes;
using Microsoft.AspNetCore.Http.HttpResults;
using Igloo.Presentation.Controllers.Dtos;
using FluentValidation;

[ApiController]
[Route("api/v1/auth")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Performs user login and returns a JWT token
    /// </summary>
    /// <response code="200">Login successful</response>
    /// <response code="400">Invalid input data</response>
    /// <response code="404">Invalid credentials</response>
    [HttpPost("login")]
    [ProducesResponseType(typeof(LoginResponse), 200)]
    [ProducesResponseType(typeof(ValidationErrorResponse), 400)]
    [ProducesResponseType(typeof(ErrorResponse), 404)]
    public async Task<IActionResult> Login([FromBody] LoginCommand command, CancellationToken cancellationToken)
    {
        try
        {
            var response = await _mediator.Send(command, cancellationToken);
            return Ok(response);
        }
        catch (ArgumentException ex)
        {
            return NotFound(new ErrorResponse { Message = ex.Message });
        }
    }
} 