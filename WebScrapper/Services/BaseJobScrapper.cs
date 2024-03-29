﻿using JobOffersApiCore.Dto;
using JobOffersApiCore.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using OpenQA.Selenium;
using WebScrapperService.Interfaces;
using WebScrapperService.Props;
using static WebScrapperService.Props.RabbitMQJobProps;


namespace WebScrapperService.Services
{
    public abstract class BaseJobScrapper
    {
        protected int PageNumber { get; set; } = 1;

        protected IWebDriver? _driver;
        protected readonly ILogger<BaseJobScrapper> _logger;
        protected readonly IJobHandleMessageProducer _jobHandleMessageProducer;
        protected readonly IWebDriverFactory _webDriverFactory;

        protected readonly string BaseUrl;
        protected readonly string JobElementOnPageSelector;
        protected readonly string JobTitleSelector;
        protected readonly string CompanySelector;
        protected readonly string LocalizationSelector;
        protected readonly string WorkModeSelector;
        protected readonly string SenioritySelector;
        protected readonly string TechnologiesSelector;
        protected readonly string SalarySelector;
        protected readonly string? LinkSelector;

        protected string FullUrl => $"{BaseUrl}{PageNumber}";

        protected BaseJobScrapper(
            ILogger<BaseJobScrapper> log,
            IJobHandleMessageProducer jobHandleMessageProducer,
            IWebDriverFactory webDriverFactory,
            string baseUrl,
            string jobElementOnPageSelector,
            string jobTitleSelector,
            string companySelector,
            string localizationSelector,
            string workModeSelector,
            string senioritySelector,
            string technologiesSelector,
            string salarySelector,
            string? linkSelector
            )
        {
            _logger = log;
            _jobHandleMessageProducer = jobHandleMessageProducer;
            _webDriverFactory = webDriverFactory;

            BaseUrl = baseUrl;
            JobElementOnPageSelector = jobElementOnPageSelector;
            JobTitleSelector = jobTitleSelector;
            CompanySelector = companySelector;
            LocalizationSelector = localizationSelector;
            WorkModeSelector = workModeSelector;
            SenioritySelector = senioritySelector;
            TechnologiesSelector = technologiesSelector;
            SalarySelector = salarySelector;
            LinkSelector = linkSelector;
        }

        public virtual void ScrapOfferts(bool isInit)
        {
            _driver = _webDriverFactory.GetWebDriver();

            try
            {
                while (true)
                {
                    if (!isInit && PageNumber > 10) break;

                    NavigateToOffersPage(FullUrl);

                    var jobElements = GetJobElementsFromPage();

                    if (jobElements.Count == 0) break;

                    IEnumerable<string> jobLinks = GetJobLinks(jobElements);

                    foreach (string jobLink in jobLinks)
                    {
                        NavigateToJobDetailPage(jobLink);

                        JobOfferRaw? offer = GetJobDetail();

                        if (offer != null)
                        {
                            _jobHandleMessageProducer.SendMessage(
                                JOB_OFFER_EXCHANGE,
                                JOB_CREATE_ROUTING_KEY,
                                offer
                                );

                            _logger.LogInformation("WebScrapperService - offer.handle event sended");
                        }
                    }

                    PageNumber++;
                }
                _driver!.Close();
                _jobHandleMessageProducer.CloseConnection();
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occured {ex}", ex);
            }
        }

        protected virtual void NavigateToOffersPage(string offersPageLink)
        {
            _driver!.Navigate().GoToUrl(offersPageLink);

            _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(2);
        }

        protected virtual void NavigateToJobDetailPage(string jobPageLink)
        {
            _driver!.Navigate().GoToUrl(jobPageLink);
        }

        protected virtual JobOfferRaw? GetJobDetail()
        {
            try
            {
                var salaryString = _driver!.FindElements(By.CssSelector(SalarySelector)).Select(e => e.Text).ToArray();
                var jobTitle = _driver.FindElement(By.CssSelector(JobTitleSelector)).Text;
                var company = _driver.FindElement(By.CssSelector(CompanySelector)).Text;
                var localization = _driver.FindElement(By.CssSelector(LocalizationSelector)).Text;
                var workMode = _driver.FindElement(By.CssSelector(WorkModeSelector)).Text;
                var seniority = _driver.FindElement(By.CssSelector(SenioritySelector)).Text;
                var techologies = _driver.FindElements(By.CssSelector(TechnologiesSelector)).Select(e => e.Text);

                company = company.Replace("About the company", "").Replace("O firmie", "");

                return new(jobTitle, company, localization, workMode,
                    seniority, techologies, _driver.Url, salaryString.Length > 0 ? salaryString[0] : null);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error ocuured when getting job details {ex}", ex);

                return null;
            }
        }

        protected virtual ICollection<IWebElement> GetJobElementsFromPage()
        {
            _logger.LogInformation("Offerts from page number {PageNumber} are utilizng", PageNumber);

            return _driver!.FindElements(By.CssSelector(JobElementOnPageSelector)).ToList();
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
