namespace Igloo.Domain.Entities;

public class GroupMembership
{
    public long GroupId { get; init; }
    public long ProfileId { get; init; }
    public DateTime JoinedAt { get; set; }

    public Group Group { get; set; } = null!;
    public Profile Profile { get; set; } = null!;
}
