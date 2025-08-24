namespace Igloo.Features.Users.CreateUser;

using MediatR;
using BCrypt.Net;
using Microsoft.EntityFrameworkCore;
using Igloo.Infrastructure.Persistence;
using Igloo.Domain.Entities;
using FluentValidation;
using Igloo.Domain.Exceptions;

public class CreateUserHandler(IglooDbContext db, IValidator<CreateUserCommand> validator) : IRequestHandler<CreateUserCommand, long>
{
    public async Task<long> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(request, cancellationToken);
        bool exists = await db.Users.AnyAsync(u => u.Email == request.Email, cancellationToken);
        if (exists)
            throw new ConflictException("Email already in use");

        var user = new User
        {
            Email = request.Email,
            PasswordHash = BCrypt.HashPassword(request.Password),
            CreatedAt = DateTime.UtcNow
        };

        db.Users.Add(user);
        await db.SaveChangesAsync(cancellationToken);

        return user.Id;
    }
}
