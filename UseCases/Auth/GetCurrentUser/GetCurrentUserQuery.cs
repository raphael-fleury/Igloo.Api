namespace Igloo.UseCases.Auth.GetCurrentUser;

using MediatR;

public record GetCurrentUserQuery : IRequest<CurrentUserResponse?>; 