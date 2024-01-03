using JobOffersApiCore.BaseObjects;
using UsersService.DbContexts;
using UsersService.Entities;
using UsersService.Interfaces;

namespace UsersService.Repository
{
    public class TechnologyRepository : BaseRepository<UsersDbContext , Technology> , ITechnologyRepository
    {
        public TechnologyRepository(UsersDbContext context):base(context)
        {
            
        }
    }
}
