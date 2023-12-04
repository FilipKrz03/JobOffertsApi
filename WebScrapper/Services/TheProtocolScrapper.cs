using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebScrapperService.Interfaces;
using OpenQA.Selenium.Support.UI;

namespace WebScrapperService.Services
{
    public class TheProtocolScrapper : IScrapperService
    {
        private int PageNumber { get; set; } = 64;
        private const string BaseUrl =
            "https://theprotocol.it/filtry/umowa-o-staz-praktyki,umowa-agencyjna,umowa-o-dzielo,umowa-na-zastepstwo,umowa-zlecenie,umowa-o-prace,kontrakt-b2b;c?pageNumber=";

        private string FullUrl => $"{BaseUrl}{PageNumber}";

        private readonly ChromeDriver _driver;
        private readonly IJavaScriptExecutor _jse;

        public TheProtocolScrapper()
        {
            _driver = new ChromeDriver();
            _jse = (IJavaScriptExecutor)_driver;
        }

        public void ScrapOfferts()
        {

            while (true)
            {
                _driver.Navigate().GoToUrl(FullUrl);

                _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);

                SecurityChecker();
                
                var jobElements = _driver.FindElements(By.CssSelector("[data-test='offersList'] [data-test='list-item-offer']")).ToList();

                if (jobElements.Count == 0) break;

                IEnumerable<string> jobLinks = GetJobLinks(jobElements);

                foreach (var jobLink in jobLinks)
                {
                    GetJobDetail(jobLink);
                }

                PageNumber++;
            }
        }

        private void GetJobDetail(string jobPageLink)
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
                Console.WriteLine(ex.Message);
            }
        }

        private IEnumerable<string> GetJobLinks(List<IWebElement> elements)
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
                    Thread.Sleep(5000);
                    _jse.ExecuteScript("document.querySelector('input').click()");
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
