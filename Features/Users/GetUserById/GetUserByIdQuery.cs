namespace Igloo.Features.Users.GetUserById;

using MediatR;
using Igloo.Features.Users.Dtos;

public record GetUserByIdQuery(long Id) : IRequest<UserDto?>;
