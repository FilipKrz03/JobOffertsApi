using JobOffersApiCore.Interfaces;
using Microsoft.Extensions.Logging;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebScrapperService.Interfaces;

namespace WebScrapperService.Services
{
    public class PracujPlScrapperService : BaseJobScrapper, IScrapperService
    {
        public PracujPlScrapperService(
            ILogger<PracujPlScrapperService> log,
            IJobHandleMessageProducer jobHandleMessageProducer,
            IWebDriverFactory webDriverFactory
            )
             : base(
                   log,
                   jobHandleMessageProducer,
                   webDriverFactory,
                   "https://it.pracuj.pl/praca?pn=",
                  ".c1fljezf",
                  "h1",
                  "[data-test='text-employerName']",
                  "[data-test='text-benefit']",
                  "[data-test='sections-benefit-work-modes-text']",
                  "[data-test='sections-benefit-employment-type-name-text']",
                  "[data-test='item-technology']",
                  "[data-test='text-earningAmount']",
                  ".core_n194fgoq"
                   )
        { }
    }
}
