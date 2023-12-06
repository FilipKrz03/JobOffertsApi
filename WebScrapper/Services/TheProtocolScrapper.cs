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

namespace WebScrapperService.Services
{
    public class TheProtocolScrapper : BaseJobScrapper, IScrapperService
    {
        private readonly IJavaScriptExecutor _jse;

        public TheProtocolScrapper(ILogger<TheProtocolScrapper> log) : base(log 
            , "https://theprotocol.it/filtry/umowa-o-staz-praktyki,umowa-agencyjna,umowa-o-dzielo,umowa-na-zastepstwo,umowa-zlecenie,umowa-o-prace,kontrakt-b2b;c?pageNumber=",
            "[data-test='offersList'] [data-test='list-item-offer']", "[data-test='text-offerTitle']",
            "[data-test='text-offerEmployer']", "[data-test='text-workplaceAddress']",
            "[data-test='section-workModes'] .rootClass_rpqnjlt", "[data-test='section-positionLevels'] .rootClass_rpqnjlt",
            "[data-test='chip-technology'] span", null)

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
            catch(Exception ex)
            {
                _logger.LogError("Error occured on SecurityChecker", ex);
            }
        }
    }
}
