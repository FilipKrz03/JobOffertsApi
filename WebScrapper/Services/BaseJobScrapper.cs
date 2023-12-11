﻿using JobOffersApiCore.Dto;
using JobOffersApiCore.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using WebScrapperService.Interfaces;
using WebScrapperService.Props;

namespace WebScrapperService.Services
{
    public abstract class BaseJobScrapper
    {
        protected int PageNumber { get; set; } = 1;

        protected readonly ChromeDriver _driver;
        protected readonly ILogger<BaseJobScrapper> _logger;
        protected readonly IRabbitMessageProducer _jobOfferMessageProducer;

        protected readonly string BaseUrl;
        protected readonly string JobElementOnPageSelector;
        protected readonly string JobTitleSelector;
        protected readonly string CompanySelector;
        protected readonly string LocalizationSelector;
        protected readonly string WorkModeSelector;
        protected readonly string SenioritySelector;
        protected readonly string TechnologiesSelector;
        protected readonly string? LinkSelector;

        protected string FullUrl => $"{BaseUrl}{PageNumber}";

        protected BaseJobScrapper(ILogger<BaseJobScrapper> log, IRabbitMessageProducer jobOfferMessageProducer,
            string baseUrl, string jobElementOnPageSelector, string jobTitleSelector,
            string companySelector, string localizationSelector, string workModeSelector,
            string senioritySelector, string technologiesSelector, string? linkSelector)
        {
            _logger = log;
            _jobOfferMessageProducer = jobOfferMessageProducer;
            _driver = new();
            BaseUrl = baseUrl;
            JobElementOnPageSelector = jobElementOnPageSelector;
            JobTitleSelector = jobTitleSelector;
            CompanySelector = companySelector;
            LocalizationSelector = localizationSelector;
            WorkModeSelector = workModeSelector;
            SenioritySelector = senioritySelector;
            TechnologiesSelector = technologiesSelector;
            LinkSelector = linkSelector;
        }

        public virtual void ScrapOfferts()
        {
            while (true)
            {
                NavigateToOffersPage(FullUrl);

                var jobElements = GetJobElementsFromPage();
             
                if (jobElements.Count == 0)
                {
                    _jobOfferMessageProducer.CloseConnection();
                    break;
                }

                IEnumerable<string> jobLinks = GetJobLinks(jobElements);

                foreach (string jobLink in jobLinks)
                {
                    NavigateToJobDetailPage(jobLink);

                    JobOfferRaw? offer = GetJobDetail();

                    if(offer != null)
                    {
                        _jobOfferMessageProducer.SendMessage
                            (RabbitMQJobProps.JOB_OFFER_EXCHANGE, RabbitMQJobProps.JOB_CREATE_ROUTING_KEY, offer);
                    }
     
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

        protected virtual JobOfferRaw? GetJobDetail()
        {
            _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(2);

            try
            {
                var jobTitle = _driver.FindElement(By.CssSelector(JobTitleSelector)).Text;
                var company = _driver.FindElement(By.CssSelector(CompanySelector)).Text;
                var localization = _driver.FindElement(By.CssSelector(LocalizationSelector)).Text;
                var workMode = _driver.FindElement(By.CssSelector(WorkModeSelector)).Text;
                var seniority = _driver.FindElement(By.CssSelector(SenioritySelector)).Text;
                var techologies = _driver.FindElements(By.CssSelector(TechnologiesSelector)).Select(e => e.Text).ToList();

                return new(jobTitle, company, localization, workMode, seniority, techologies , _driver.Url);
            }
            catch(Exception ex)
            {
                _logger.LogError("Error ocuured when getting job details {ex}", ex);

                return null;
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
