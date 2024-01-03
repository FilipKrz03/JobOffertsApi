using AutoMapper;
using UsersService.Dto;
using UsersService.Entities;

namespace UsersService.Profiles
{
    public class TechnologyProfile : Profile
    {
        public TechnologyProfile()
        {
            CreateMap<Technology, TechnologyBasicResponseDto>();
        }
    }
}
