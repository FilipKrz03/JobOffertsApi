using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebScrapperService.Interfaces;
using OpenQA.Selenium.Support.UI;
using Microsoft.Extensions.Logging;
using JobOffersApiCore.Interfaces;

namespace WebScrapperService.Services
{
    public class TheProtocolScrapperService : BaseJobScrapper, IScrapperService
    {
        private readonly IJavaScriptExecutor _jse;

        public TheProtocolScrapperService(
            ILogger<TheProtocolScrapperService> log,
            IJobHandleMessageProducer jobHandleMessageProducer,
            IWebDriverFactory webDriverFactory
            )
            : base(
                  log,
                  jobHandleMessageProducer,
                  webDriverFactory,
                  "https://theprotocol.it/filtry/umowa-o-staz-praktyki,umowa-agencyjna,umowa-o-dzielo,umowa-na-zastepstwo,umowa-zlecenie,umowa-o-prace,kontrakt-b2b;c/praca/bi-developer-warszawa-chodna-51,oferta,af880000-408b-5232-da75-08dbfc7d526d?s=-21349304240&searchId=e17b80d0-9a76-11ee-b506-13b33335b357&sort=date&pageNumber=",
                  "[data-test='offersList'] [data-test='list-item-offer']",
                  "[data-test='text-offerTitle']",
                  "[data-test='text-offerEmployer']",
                  "[data-test='text-workplaceAddress']",
                  "[data-test='section-workModes']",
                  "[data-test='section-positionLevels']",
                  "[data-test='chip-technology']",
                  "[data-test='text-contractSalary']",
                   null
                   )
        {
            _jse = (IJavaScriptExecutor)_driver;
        }

        protected override IEnumerable<string> GetJobLinks(ICollection<IWebElement> elements)
        {
            ICollection<string> links = new List<string>();

            foreach (var element in elements)
            {
                var href = element.GetAttribute("href");

                links.Add(href);
            }
            return links;
        }

        protected override void NavigateToOffersPage(string offersPageLink)
        {
            base.NavigateToOffersPage(offersPageLink);
            SecurityChecker();
        }

        protected override void NavigateToJobDetailPage(string jobPageLink)
        {
            base.NavigateToJobDetailPage(jobPageLink);
            SecurityChecker();
            _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
        }

        private void SecurityChecker()
        {
            try
            {
                var h1 = _driver.FindElement(By.CssSelector("h1")).Text;

                if (h1 == "theprotocol.it")
                {
                    Task.Delay(4000).Wait();
                    _jse.ExecuteScript("document.querySelector('input').click()");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occured on SecurityChecker", ex);
            }
        }
    }
}
