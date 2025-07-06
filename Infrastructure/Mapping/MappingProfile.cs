using AutoMapper;
using Igloo.Domain.Entities;
using Igloo.UseCases.Auth.GetCurrentUser;
using Igloo.UseCases.Profiles.Dtos;

namespace Igloo.Infrastructure.Mapping;

public class MappingProfile : AutoMapper.Profile
{
    public MappingProfile()
    {
        CreateMap<User, CurrentUserResponse>();
    }
}
