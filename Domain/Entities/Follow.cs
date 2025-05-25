namespace Igloo.Domain.Entities;

public class Follow
{
    public long FollowerProfileId { get; init; }
    public long FollowedProfileId { get; init; }
    public DateTime CreatedAt { get; set; }

    public Profile FollowerProfile { get; set; } = null!;
    public Profile FollowedProfile { get; set; } = null!;
}
