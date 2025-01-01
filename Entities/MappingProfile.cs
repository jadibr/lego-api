using AutoMapper;

namespace lego_api;

public class MappingProfile : Profile
{
  public MappingProfile()
  {
    CreateMap<BrickEntity, Brick>();
    CreateMap<Brick, BrickEntity>();
    CreateMap<UserEntity, User>()
      .ForMember(u => u.Password, opt => opt.Ignore());
  }
}