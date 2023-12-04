using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebScrapperService.Interfaces;

namespace WebScrapperService.Services
{
    public class NoFluffJobsScraper : IScrapperService
    {
        private int PageNumber { get; set; } = 1;
        private const string BaseUrl = "https://nofluffjobs.com/pl/?criteria=salary%3Cpln49900m&page=";
    
        private string FullUrl => $"{BaseUrl}{PageNumber}";

        private readonly ChromeDriver _driver;

        public NoFluffJobsScraper()
        {
            _driver = new ChromeDriver();
        }

        public void ScrapOfferts()
        {
            EnsureProperTrust();

            while (true)
            {
                _driver.Navigate().GoToUrl(FullUrl);

                var jobElements = _driver.FindElements(By.CssSelector(".posting-list-item")).ToList();

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

            try
            {
                var jobTitle = _driver.FindElement(By.TagName("h1")).Text;
                var company = _driver.FindElement(By.TagName("h2")).Text;

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

        private void EnsureProperTrust()
        {
            try
            {
                _driver.Navigate().GoToUrl("https://nofluffjobs.com/pl");
                _driver.FindElement(By.CssSelector("#onetrust-accept-btn-handler")).Click();
                _driver.FindElements(By.CssSelector(".region-control")).FirstOrDefault()?.Click();
                _driver.FindElement(By.CssSelector(".mn-3")).Click();
                _driver.Manage().Cookies.AddCookie(new Cookie("personalization_id", "v1_IpPfSw2dGvfLFuuzNzw/yw=="));
                _driver.Navigate().GoToUrl("https://nofluffjobs.com/pl/?criteria=salary%3Cpln49900m&page=5");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }
    }
}



