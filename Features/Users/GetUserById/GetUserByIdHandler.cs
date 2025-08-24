namespace Igloo.Features.Users.GetUserById;

using MediatR;
using Microsoft.EntityFrameworkCore;
using Igloo.Infrastructure.Persistence;
using Igloo.Features.Users.Dtos;

public class GetUserByIdHandler(IglooDbContext dbContext) : IRequestHandler<GetUserByIdQuery, UserDto?>
{
    public async Task<UserDto?> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var user = await dbContext.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == request.Id, cancellationToken);

        if (user is null) return null;

        return new UserDto(user.Id, user.Email, user.CreatedAt);
    }
}
