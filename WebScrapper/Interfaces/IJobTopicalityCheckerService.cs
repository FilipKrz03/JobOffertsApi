﻿using JobOffersApiCore.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebScrapperService.Interfaces
{
    public interface IJobTopicalityCheckerService
    {
        void CheckIfOfferStillExist(string message);
    }
}
