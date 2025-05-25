namespace UseCases.Users.CreateUser;

using MediatR;

public class CreateUserCommand : IRequest<long>
{
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
}