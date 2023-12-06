using Microsoft.Extensions.Logging;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebScrapperService.Dto;
using WebScrapperService.Interfaces;

namespace WebScrapperService.Services
{
    public class PracujPlScrapper : BaseJobScrapper, IScrapperService
    {
        public PracujPlScrapper(ILogger<PracujPlScrapper> log , IMessageProducer<JobOffer> jobOfferMessageProducer)
             : base(log , jobOfferMessageProducer , "https://it.pracuj.pl/praca?pn=",
                  ".c1fljezf", "h1", "h2" , "[data-test='text-benefit']", "[data-test='sections-benefit-work-modes-text']",
               "[data-test='sections-benefit-employment-type-name-text']", "[data-test='item-technology']", ".core_n194fgoq"){ }
    }
}
