using AutoMapper;
using JobOffersApiCore.Common;

namespace JobOffersService.Profiles
{
    public class PagedListProfile : Profile
    {
        public PagedListProfile()
        {
            CreateMap(typeof(PagedList<>), typeof(PagedList<>)).ConvertUsing(typeof(PagedListConverter<,>));
        }
    }
}
