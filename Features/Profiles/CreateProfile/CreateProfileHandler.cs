namespace Igloo.Features.Profiles.CreateProfile;

using MediatR;
using Microsoft.EntityFrameworkCore;
using Igloo.Infrastructure.Persistence;
using Igloo.Domain.Entities;
using FluentValidation;
using Igloo.Domain.Exceptions;

public class CreateProfileHandler(IglooDbContext db, IValidator<CreateProfileCommand> validator) : IRequestHandler<CreateProfileCommand, long>
{
    public async Task<long> Handle(CreateProfileCommand request, CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(request, cancellationToken);
        
        bool usernameExists = await db.Profiles.AnyAsync(p => p.Username == request.Username, cancellationToken);
        if (usernameExists)
            throw new ConflictException("Username already in use");

        var profile = new Profile
        {
            Username = request.Username,
            DisplayName = request.DisplayName,
            Bio = request.Bio,
            IsDeactivated = false,
            CreatedAt = DateTime.UtcNow
        };

        db.Profiles.Add(profile);
        await db.SaveChangesAsync(cancellationToken);

        return profile.Id;
    }
} 