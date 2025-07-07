namespace Igloo.Presentation.Controllers.Dtos;

public record IdResponse(long Id)
{
    public static IdResponse From(long id) => new(id);
}