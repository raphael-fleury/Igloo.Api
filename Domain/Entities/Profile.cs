namespace Igloo.Domain.Entities;

public class Profile
{
    public long Id { get; init; }
    public string Username { get; set; } = null!;
    public string DisplayName { get; set; } = null!;
    public string? Bio { get; set; }
    public bool IsDeactivated { get; set; }
    public DateTime CreatedAt { get; set; }

    public ICollection<UserProfile> UserProfiles { get; set; } = new List<UserProfile>();
    public ICollection<Follow> Followers { get; set; } = new List<Follow>();
    public ICollection<Follow> Followings { get; set; } = new List<Follow>();
    public ICollection<Mute> Muters { get; set; } = new List<Mute>();
    public ICollection<Mute> Muteds { get; set; } = new List<Mute>();
    public ICollection<Post> Posts { get; set; } = new List<Post>();
    public ICollection<GroupMembership> GroupMemberships { get; set; } = new List<GroupMembership>();
}
