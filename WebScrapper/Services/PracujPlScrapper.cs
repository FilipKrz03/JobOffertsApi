using HtmlAgilityPack;
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
    public class PracujPlScrapper : IScrapperService
    {
        private int PageNumber { get; set; } = 1;  
        private const string BaseUrl = "https://it.pracuj.pl/praca?pn=";

        private string FullUrl => $"{BaseUrl}{PageNumber}";

        private readonly ChromeDriver _driver;

        public PracujPlScrapper()
        {
            _driver = new ChromeDriver();
        }

        public void ScrapOfferts()
        {
            while(true)
            {
                _driver.Navigate().GoToUrl(FullUrl);

                var jobElements = _driver.FindElements(By.ClassName("c1fljezf")).ToList();

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
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            } 
        }

        private IEnumerable<string> GetJobLinks(List<IWebElement> elements)
        {
            ICollection<string> links = new List<string>();

            foreach (var element in elements)
            {
                var href = element.FindElement(By.ClassName("core_n194fgoq")).GetAttribute("href");

                links.Add(href);
            }

            return links;
        }
    }
}
