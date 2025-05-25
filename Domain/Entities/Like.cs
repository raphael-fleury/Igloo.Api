namespace Domain.Entities;

public class Like
{
    public long ProfileId { get; init; }
    public long PostId { get; init; }
    public DateTime CreatedAt { get; set; }

    public Profile Profile { get; set; } = null!;
    public Post Post { get; set; } = null!;
}
