using Microsoft.Extensions.Logging;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebScrapperService.Services
{
    public abstract class BaseJobScrapper
    {
        protected int PageNumber { get; set; } = 1;

        protected readonly string BaseUrl;
        protected readonly string JobElementOnPageSelector;
        protected readonly string JobTitleSelector;
        protected readonly string CompanySelector;
        protected readonly string LocalizationSelector;
        protected readonly string? LinkSelector;

        protected readonly ChromeDriver _driver;
        protected readonly ILogger<BaseJobScrapper> _logger;

        protected string FullUrl => $"{BaseUrl}{PageNumber}";

        protected BaseJobScrapper(ILogger<BaseJobScrapper> log, string baseUrl, string jobElementOnPageSelector, string jobTitleSelector,
            string companySelector, string localizationSelector, string? linkSelector)
        {
            _logger = log;
            _driver = new();
            BaseUrl = baseUrl;
            JobElementOnPageSelector = jobElementOnPageSelector;
            JobTitleSelector = jobTitleSelector;
            CompanySelector = companySelector;
            LocalizationSelector = localizationSelector;
            LinkSelector = linkSelector;
        }

        public virtual void ScrapOfferts()
        {
            while (true)
            {
                NavigateToOffersPage(FullUrl);

                var jobElements = GetJobElementsFromPage();

                if (jobElements.Count == 0) break;

                IEnumerable<string> jobLinks = GetJobLinks(jobElements);

                foreach (string jobLink in jobLinks)
                {
                    NavigateToJobDetailPage(jobLink);
                    GetJobDetail();
                }

                PageNumber++;
            }
        }

        protected virtual void NavigateToOffersPage(string offersPageLink)
        {
            _driver.Navigate().GoToUrl(offersPageLink);

            _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(2);
        }

        protected virtual void NavigateToJobDetailPage(string jobPageLink)
        {
            _driver.Navigate().GoToUrl(jobPageLink);
        }

        protected virtual void GetJobDetail()
        {
            _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(2);

            try
            {
                var jobTitle = _driver.FindElement(By.CssSelector(JobTitleSelector)).Text;
                var company = _driver.FindElement(By.CssSelector(CompanySelector)).Text;
                var localization = _driver.FindElement(By.CssSelector(LocalizationSelector)).Text;

                Console.WriteLine($"Job title {jobTitle} , comapny {company} , localization {localization}");
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occured on GetJobDetailMethod", ex);
            }
        }

        protected virtual ICollection<IWebElement> GetJobElementsFromPage()
        {
            _logger.LogInformation("Offerts from page number {PageNumber} are utiling", PageNumber);

            return _driver.FindElements(By.CssSelector(JobElementOnPageSelector)).ToList();
        }

        protected virtual IEnumerable<string> GetJobLinks(ICollection<IWebElement> elements)
        {
            ICollection<string> links = new List<string>();

            foreach (var element in elements)
            {
                var href = element.FindElement(By.CssSelector(LinkSelector)).GetAttribute("href");

                links.Add(href);
            }

            return links;
        }
    }
}
