namespace Igloo.UseCases.Users.CreateUser;

using MediatR;

public record CreateUserCommand(string Email, string Password) : IRequest<long>;