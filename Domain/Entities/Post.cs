using Domain.Enums;

namespace Domain.Entities;

public class Post
{
    public long Id { get; init; }
    public long ProfileId { get; init; }
    public string? Text { get; set; }
    public PostVisibility Visibility { get; set; }
    public DateTime CreatedAt { get; set; }
    public long? ReplyToPostId { get; set; }

    public Profile Profile { get; set; } = null!;
    public Post? ReplyToPost { get; set; }
    public Group? Group { get; set; }
    public ICollection<Post> Replies { get; set; } = new List<Post>();
    public ICollection<Like> Likes { get; set; } = new List<Like>();
}
