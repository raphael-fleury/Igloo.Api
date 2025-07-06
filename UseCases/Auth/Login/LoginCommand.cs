namespace Igloo.UseCases.Auth.Login;

using MediatR;

public record LoginCommand(string Email, string Password) : IRequest<LoginResponse>; 