namespace Igloo.Domain.Entities;

public class UserProfile
{
    public long UserId { get; init; }
    public long ProfileId { get; init; }
    public string? Nickname { get; set; }
    public string Role { get; set; } = "admin";

    public User User { get; set; } = null!;
    public Profile Profile { get; set; } = null!;
}
