﻿using JobOffersApiCore.BaseObjects;
using JobOffersApiCore.Dto;
using JobOffersApiCore.Interfaces;
using JobOffersMapperService.DbContexts;
using JobOffersMapperService.Entites;
using JobOffersMapperService.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobOffersMapperService.Repository
{
    public class JobOffersBaseRepository : BaseRepository<OffersBaseContext, JobOfferBase>, IJobOffersBaseRepository
    {

        public JobOffersBaseRepository(OffersBaseContext context) : base(context) { }

        public async Task<bool> OfferExistAsync(JobOfferRaw offer)
        {
            return await Query().
             AnyAsync(j => j.OfferCompany == offer.OfferCompany && j.OfferTitle == offer.OfferTitle);
        }

        public async Task<IEnumerable<JobOfferWithIdTitleLinkDto>>
            GetAllJobOffersWithIdTitleLinkAsync()
        {
            return await
                 Query()
                .Select(x => new JobOfferWithIdTitleLinkDto(x.Id, x.OfferTitle, x.OfferLink))
                .ToListAsync();
        }
    }
}
