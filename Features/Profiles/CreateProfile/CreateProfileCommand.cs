namespace Igloo.Features.Profiles.CreateProfile;

using MediatR;

public record CreateProfileCommand(string Username, string DisplayName, string? Bio) : IRequest<long>; 