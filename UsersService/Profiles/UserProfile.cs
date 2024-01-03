using AutoMapper;
using UsersService.Dto;
using UsersService.Entities;

namespace UsersService.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<RegisterRequestDto, User>()
                .ForMember(x => x.Id, opt => opt.MapFrom(src => Guid.NewGuid()))
                .ForMember(x => x.CreatedAt, opt => opt.Ignore())
                .ForMember(x => x.UpdatedAt, opt => opt.Ignore())
                .ForMember(x => x.JobOffers, opt => opt.Ignore());
        }
    }
}
