namespace Igloo.UseCases.Profiles.CreateProfile;

using MediatR;
using Microsoft.EntityFrameworkCore;
using Igloo.Infrastructure.Persistence;
using Igloo.Domain.Entities;
using FluentValidation;

public class CreateProfileHandler : IRequestHandler<CreateProfileCommand, long>
{
    private readonly IglooDbContext _db;
    private readonly IValidator<CreateProfileCommand> _validator;

    public CreateProfileHandler(IglooDbContext db, IValidator<CreateProfileCommand> validator)
    {
        _db = db;
        _validator = validator;
    }

    public async Task<long> Handle(CreateProfileCommand request, CancellationToken cancellationToken)
    {
        await _validator.ValidateAndThrowAsync(request, cancellationToken);
        
        bool usernameExists = await _db.Profiles.AnyAsync(p => p.Username == request.Username, cancellationToken);
        if (usernameExists)
            throw new ArgumentException("Username already in use");

        var profile = new Profile
        {
            Username = request.Username,
            DisplayName = request.DisplayName,
            Bio = request.Bio,
            IsDeactivated = false,
            CreatedAt = DateTime.UtcNow
        };

        _db.Profiles.Add(profile);
        await _db.SaveChangesAsync(cancellationToken);

        return profile.Id;
    }
} 