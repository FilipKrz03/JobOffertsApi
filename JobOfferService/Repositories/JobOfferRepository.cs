﻿using JobOffersApiCore.BaseObjects;
using JobOffersApiCore.Common;
using JobOffersService.DbContexts;
using JobOffersService.Entities;
using JobOffersService.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace JobOffersService.Repositories
{
    public class JobOfferRepository : BaseRepository<JobOffersContext, JobOffer>, IJobOfferRepository
    {
        public JobOfferRepository(JobOffersContext context) : base(context) { }

        public async Task<bool> IsDatabaseInitalized()
        {
            return await Query().AnyAsync();
        }

        public async Task<IEnumerable<JobOffer>> GetJobOffersAsync(ResourceParamethers resourceParamethers , 
            Expression<Func<JobOffer , object>> keySelector)
        {
            var query = Query();

            if(!string.IsNullOrEmpty(resourceParamethers.SearchQuery))
            {
                query = query.Where(x =>
                x.OfferTitle.Contains(resourceParamethers.SearchQuery)
                || x.OfferLink.Contains(resourceParamethers.SearchQuery)
                || x.OfferCompany.Contains(resourceParamethers.SearchQuery)
                || x.Localization.Contains(resourceParamethers.SearchQuery)
                || x.Technologies.Any(t => t.TechnologyName.Contains(resourceParamethers.SearchQuery))
                );
            }

            if(resourceParamethers.SortOrder.ToLower() == "desc")
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
