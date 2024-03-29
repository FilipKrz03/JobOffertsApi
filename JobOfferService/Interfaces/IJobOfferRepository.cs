﻿using JobOffersApiCore.Common;
using JobOffersApiCore.Interfaces;
using JobOffersService.Entities;
using System.Linq.Expressions;

namespace JobOffersService.Interfaces
{
    public interface IJobOfferRepository : IBaseRepository<JobOffer>
    {
        Task<bool> IsDatabaseInitalizedAsync();
        Task<PagedList<JobOffer>> GetJobOffersAsync(ResourceParamethers resourceParamethers ,
            Expression<Func<JobOffer, object>> keySelector);
        Task<JobOffer?> GetJobOfferWithTechnologies(Guid id);
    }
}
