namespace Igloo.Features.Auth.Login;

using MediatR;

public record LoginCommand(string Email, string Password) : IRequest<LoginResponse>; 