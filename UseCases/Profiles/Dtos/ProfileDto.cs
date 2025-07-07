namespace Igloo.UseCases.Profiles.Dtos;

public record ProfileDto(
    long Id,
    string Username,
    string DisplayName,
    string? Bio,
    DateTime CreatedAt
); 