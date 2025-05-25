namespace Igloo.UseCases.Users.Dtos;

public class UserDto
{
    public long Id { get; set; }
    public string Email { get; set; } = "";
    public DateTime CreatedAt { get; set; }
}
