namespace Domain.Entities;

public class User
{
    public long Id { get; init; }
    public string Email { get; set; } = null!;
    public string PasswordHash { get; set; } = null!;
    public DateTime CreatedAt { get; set; }

    public ICollection<UserProfile> UserProfiles { get; set; } = new List<UserProfile>();
}
