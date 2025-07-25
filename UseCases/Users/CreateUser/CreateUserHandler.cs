namespace Igloo.UseCases.Users.CreateUser;

using MediatR;
using BCrypt.Net;
using Microsoft.EntityFrameworkCore;
using Igloo.Infrastructure.Persistence;
using Igloo.Domain.Entities;
using FluentValidation;
using Igloo.Domain.Exceptions;

public class CreateUserHandler : IRequestHandler<CreateUserCommand, long>
{
    private readonly IglooDbContext _db;
    private readonly IValidator<CreateUserCommand> _validator;

    public CreateUserHandler(IglooDbContext db, IValidator<CreateUserCommand> validator)
    {
        _db = db;
        _validator = validator;
    }

    public async Task<long> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        await _validator.ValidateAndThrowAsync(request, cancellationToken);
        bool exists = await _db.Users.AnyAsync(u => u.Email == request.Email, cancellationToken);
        if (exists)
            throw new ConflictException("Email already in use");

        var user = new User
        {
            Email = request.Email,
            PasswordHash = BCrypt.HashPassword(request.Password),
            CreatedAt = DateTime.UtcNow
        };

        _db.Users.Add(user);
        await _db.SaveChangesAsync(cancellationToken);

        return user.Id;
    }
}
