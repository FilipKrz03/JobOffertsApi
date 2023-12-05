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
            "[data-test='offersList'] [data-test='list-item-offer']", "[data-test='text-offerTitle']", "[data-test='text-offerEmployer']", null)

        {
            _jse = (IJavaScriptExecutor)_driver;
        }

        public new void ScrapOfferts()
        {
            while (true)
            {
                _driver.Navigate().GoToUrl(FullUrl);

                SecurityChecker();

                _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);

                var jobElements = GetJobElementsFromPage();

                if (jobElements.Count == 0) break;

                IEnumerable<string> jobLinks = GetJobLinks(jobElements);

                foreach (var jobLink in jobLinks)
                {
                    GetJobDetail(jobLink);
                }

                PageNumber++;
            }
        }

        protected new void GetJobDetail(string jobPageLink)
        {
            _driver.Navigate().GoToUrl(jobPageLink);

            SecurityChecker();

            _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);

            try
            {
                var jobTitle = _driver.FindElement(By.CssSelector("[data-test='text-offerTitle']")).Text;
                var company = _driver.FindElement(By.CssSelector("[data-test='text-offerEmployer']")).Text;

                Console.WriteLine($"Job title {jobTitle} , comapny {company}");
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occured on GetJobDetailMethod", ex);
            }
        }

        private new IEnumerable<string> GetJobLinks(ICollection<IWebElement> elements)
        {
            ICollection<string> links = new List<string>();

            foreach (var element in elements)
            {
                var href = element.GetAttribute("href");

                links.Add(href);
            }
            return links;
        }

        private void SecurityChecker()
        {
            try
            {
                var h1 = _driver.FindElement(By.CssSelector("h1")).Text;

                if (h1 == "theprotocol.it")
                {
                    Task.Delay(2000).Wait();
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
