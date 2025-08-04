namespace Igloo.Features.Users.CreateUser;

using MediatR;

public record CreateUserCommand(string Email, string Password) : IRequest<long>;