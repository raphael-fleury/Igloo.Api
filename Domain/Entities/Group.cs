namespace Domain.Entities;

public class Group
{
    public long Id { get; init; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public bool IsPublic { get; set; }
    public DateTime CreatedAt { get; set; }

    public ICollection<GroupMembership> GroupMemberships { get; set; } = new List<GroupMembership>();
}
