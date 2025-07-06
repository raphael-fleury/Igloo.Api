namespace Igloo.UseCases.Auth.GetCurrentUser;

public record CurrentUserResponse(
    long Id,
    string Email,
    DateTime CreatedAt
); 