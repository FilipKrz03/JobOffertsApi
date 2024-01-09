using JobOffersApiCore.Dto;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebScrapperService.Interfaces;

namespace WebScrapperService.Services
{
    public sealed class JobTopicalityCheckerService : IJobTopicalityCheckerService
    {

        private readonly IWebDriverFactory _webDriverFactory;
        private IWebDriver _driver;
        private readonly ILogger<JobTopicalityCheckerService> _logger;

        public JobTopicalityCheckerService(
            IWebDriverFactory webDriverFactory,
            ILogger<JobTopicalityCheckerService> logger
            )
        {
            _webDriverFactory = webDriverFactory;
            _logger = logger;
            _driver = _webDriverFactory.GetWebDriver();
        }

        public void CheckIfOfferStillExist(string message)
        {

            var offer = JsonConvert.DeserializeObject<JobOfferWithIdTitleLinkDto>(message);

            if (offer == null)
            {
                return;
            }

            try
            {
                _ = _driver.Url;
            }
            catch (WebDriverException e)
            {
                _logger.LogError("Driver exception {e}", e);
                _driver = _webDriverFactory.GetWebDriver();
            }

            string? titleSelector = offer.OfferLink switch
            {
                _ when offer.OfferLink.ToLower().Contains("theprotocol") => "[data-test='text-offerTitle']",
                _ when offer.OfferLink.ToLower().Contains("pracuj.pl") => "h1",
                _ => null
            };

            if (titleSelector == null)
            {
                _logger.LogWarning
                    ("Not recognized any title selector , Link : {offerLink}", offer.OfferLink);

                return;
            }

            _driver.Navigate().GoToUrl(offer.OfferLink);

            var offerTitleFromLink = _driver.FindElements(By.CssSelector(titleSelector));

            if (!offerTitleFromLink.Any() || offerTitleFromLink.First().Text != offer.OfferTitle)
            {
                // send delete event
            }
        }
    }
}
