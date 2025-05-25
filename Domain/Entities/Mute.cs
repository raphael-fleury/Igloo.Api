namespace Igloo.Domain.Entities;

public class Mute
{
    public long MuterProfileId { get; init; }
    public long MutedProfileId { get; init; }
    public DateTime CreatedAt { get; set; }

    public Profile MuterProfile { get; set; } = null!;
    public Profile MutedProfile { get; set; } = null!;
}
