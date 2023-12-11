﻿using JobOffersApiCore.Dto;
using JobOffersApiCore.Interfaces;
using JobOffersMapperService.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobOffersMapperService.Interfaces
{
    public interface IOffersBaseRepository : IBaseRepository<JobOfferBase>
    {
        Task<bool> OfferExistAsync(JobOfferRaw offer);

    }
}
