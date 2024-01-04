using JobOffersApiCore.BaseObjects;
using JobOffersApiCore.Common;
using System.Linq.Expressions;
using UsersService.DbContexts;
using UsersService.Entities;
using UsersService.Interfaces.RepositoriesInterfaces;

namespace UsersService.Repository
{
    public class TechnologyRepository : BaseRepository<UsersDbContext, Technology>, ITechnologyRepository
    {
        public TechnologyRepository(UsersDbContext context) : base(context) { }

        public async Task<PagedList<Technology>> GetUserTechnologiesAsync
           (ResourceParamethers resourceParamethers, Expression<Func<Technology, object>> keySelector , Guid userId)
        {
            var query = Query();

            query = query.Where(x => x.Users.Any(u => u.Id == userId));

            if (!string.IsNullOrEmpty(resourceParamethers.SearchQuery))
            {
                query = query.Where(t => t.TechnologyName.Contains(resourceParamethers.SearchQuery));
            }

            if (resourceParamethers.SortOrder == "desc")
            {
                query = query.OrderByDescending(keySelector);
            }
            else
            {
                query = query.OrderBy(keySelector);
            }

            return await PagedList<Technology>
                .CreateAsync(query, resourceParamethers.PageSize, resourceParamethers.PageNumber);
        }
    }
}
