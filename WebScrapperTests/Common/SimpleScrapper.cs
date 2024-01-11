using JobOffersApiCore.Interfaces;
using Microsoft.Extensions.Logging;
using Moq;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebScrapperService.Interfaces;
using WebScrapperService.Services;

namespace WebScrapperTests.Common
{
    public class SimpleScrapper : BaseJobScrapper
    {
        public SimpleScrapper
            (ILogger<SimpleScrapper>logger , IJobHandleMessageProducer messageProdcuer, IWebDriverFactory webDriverFactory )
            :base(logger , messageProdcuer, webDriverFactory , "" , "" ,"" ,"" ,"" , "" , "" ,"" ,"" , "") { }
    }
}
