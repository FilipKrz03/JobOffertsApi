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
                .ForMember(x => x.CreatedAt, opt => opt.Ignore())
                .ForMember(x => x.UpdatedAt, opt => opt.Ignore())
                .ForMember(x => x.Id, opt => opt.Ignore())
                .ForMember(x => x.IdentityId, opt => opt.Ignore())
                .ForMember(x => x.FavouriteOffers, opt => opt.Ignore());
        }
    }
}
