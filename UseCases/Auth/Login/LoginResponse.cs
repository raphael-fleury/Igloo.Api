namespace Igloo.UseCases.Auth.Login;

public record LoginResponse(
    string Token,
    string RefreshToken,
    DateTime ExpiresAt,
    long UserId,
    string Email,
    long? ActiveProfileId
); 