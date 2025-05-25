namespace Igloo.UseCases.Users.GetUserById;

using MediatR;
using Igloo.UseCases.Users.Dtos;

public record GetUserByIdQuery(long Id) : IRequest<UserDto?>;
