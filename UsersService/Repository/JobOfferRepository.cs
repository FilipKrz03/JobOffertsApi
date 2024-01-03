using JobOffersApiCore.BaseObjects;
using JobOffersApiCore.Common;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using UsersService.DbContexts;
using UsersService.Entities;
using UsersService.Interfaces;

namespace UsersService.Repository
{
    public class JobOfferRepository : BaseRepository<UsersDbContext, JobOffer>, IJobOfferRepository
    {
        public JobOfferRepository(UsersDbContext context) : base(context) { }

        public async Task<JobOffer?> GetUserJobOffer(Guid userId, Guid jobOfferId)
        {
            return await Query()
                .Where(x => x.Id == jobOfferId && x.Users.Any(x => x.Id == userId))
                .Include(x => x.Technologies)
                .FirstOrDefaultAsync();
        }

        public async Task<PagedList<JobOffer>> GetUserJobOffersAsync
            (Expression<Func<JobOffer , object>> keySelector , ResourceParamethers resourceParamethers , Guid userId)
        {
            var query = Query();

            // Only favourite offers owned by user with delivered Id
            query = query.Where(x => x.Users.Any(x => x.Id == userId));

            if (!string.IsNullOrEmpty(resourceParamethers.SearchQuery))
            {
                query = query.Where(x =>
                x.OfferTitle.Contains(resourceParamethers.SearchQuery)
                || x.OfferLink.Contains(resourceParamethers.SearchQuery)
                || x.OfferCompany.Contains(resourceParamethers.SearchQuery)
                || x.Localization.Contains(resourceParamethers.SearchQuery)
                || x.Technologies.Any(t => t.TechnologyName.Contains(resourceParamethers.SearchQuery))
                );
            }

            if (resourceParamethers.SortOrder.ToLower() == "desc")
            {
                query = query.OrderByDescending(keySelector);
            }
            else
            {
                query = query.OrderBy(keySelector);
            }

            return await PagedList<JobOffer>
                .CreateAsync(query, resourceParamethers.PageSize, resourceParamethers.PageNumber);
        }
    }
}
