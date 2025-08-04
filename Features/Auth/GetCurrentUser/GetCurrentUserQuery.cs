namespace Igloo.Features.Auth.GetCurrentUser;

using MediatR;

public record GetCurrentUserQuery : IRequest<CurrentUserResponse?>; 