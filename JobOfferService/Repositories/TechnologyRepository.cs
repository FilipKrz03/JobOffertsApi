using JobOffersApiCore.BaseObjects;
using JobOffersApiCore.Common;
using JobOffersService.DbContexts;
using JobOffersService.Entities;
using JobOffersService.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace JobOffersService.Repositories
{
    public class TechnologyRepository : BaseRepository<JobOffersContext, Technology>, ITechnologyRepository
    {
        public TechnologyRepository(JobOffersContext context) : base(context) { }

        public async Task<IEnumerable<Technology>> GetAllTechnologiesAsync()
        {
            return await Query().ToListAsync();
        }

        public async Task<List<Technology>> GetEntitiesFromTechnologiesNamesAsync(IEnumerable<string> technologyNames)
        {
            return await Query().Where
                (t => technologyNames.Any(tn => t.TechnologyName.ToLower() == tn.ToLower())).ToListAsync();
        }

        public async Task<IEnumerable<Technology>> GetTechnologiesAsync
            (ResourceParamethers resourceParamethers ,Expression<Func<Technology , object>> keySelector)
        {
            var query = Query();

            if(!string.IsNullOrEmpty(resourceParamethers.SearchQuery))
            {
                query = query.Where(t => t.TechnologyName.Contains(resourceParamethers.SearchQuery));
            }

            if(resourceParamethers.SortOrder == "desc")
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

        public async Task<Technology>
            GetTechnologyWithJobOffersAsync(Guid id , ResourceParamethers resourceParamethers)
        {
            return await GetByIdQuery(id)
                .Include(
                t => t.JobOffers
                .Skip((resourceParamethers.PageNumber - 1) * resourceParamethers.PageSize)
                .Take(resourceParamethers.PageSize)
                .OrderBy(o => o.Id)
                )
                .SingleAsync();
        }
    }
}
