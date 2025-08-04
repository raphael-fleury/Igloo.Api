using AutoMapper;
using Igloo.Domain.Entities;
using Igloo.Features.Auth.GetCurrentUser;
using Igloo.Features.Profiles.Dtos;

namespace Igloo.Infrastructure.Mapping;

public class MappingProfile : AutoMapper.Profile
{
    public MappingProfile()
    {
        CreateMap<User, CurrentUserResponse>();
    }
}
