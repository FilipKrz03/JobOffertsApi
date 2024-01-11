using Castle.Core.Logging;
using JobOffersApiCore.Dto;
using JobOffersApiCore.Interfaces;
using Microsoft.Extensions.Logging;
using Moq;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebScrapperService.Interfaces;
using WebScrapperService.Services;
using WebScrapperTests.Common;

namespace WebScrapperTests.Services
{
    public class ScrapperTests
    {
        private readonly Mock<ILogger<SimpleScrapper>> _loggerMock;
        private readonly Mock<IJobHandleMessageProducer> _rabbitMessageProducerMock;
        private readonly Mock<IWebDriver> _driverMock;
        private readonly SimpleScrapper _simpleScrapper;

        public ScrapperTests()
        {
            _driverMock = new();

            Mock<IWebDriverFactory> factoryMock = new();
            factoryMock.Setup(x => x.GetWebDriver())
                .Returns(_driverMock.Object);

            _rabbitMessageProducerMock = new();
            _loggerMock = new();

            _driverMock.Setup(x => x.Navigate().GoToUrl("url"));
            _driverMock.Setup(x => x.Manage().Timeouts().ImplicitWait);

            _simpleScrapper = new
                (_loggerMock.Object,
                _rabbitMessageProducerMock.Object,
                factoryMock.Object
                );
        }

        [Fact]
        public void Scrapper_Should_CloseRabbitConnection_WhenNoMoreOffers()
        {
            IList<IWebElement> elements = new List<IWebElement>();

            System.Collections.ObjectModel.ReadOnlyCollection<IWebElement> enumerableToReutrn
                = new(elements);

            _driverMock.Setup(x => x.FindElements(By.CssSelector("")))
                .Returns(enumerableToReutrn);

            _simpleScrapper.ScrapOfferts(false);

            _rabbitMessageProducerMock.Verify(x => x.CloseConnection(), Times.Once);
        }

        [Fact]
        public void Scrapper_Should_CloseWebDriverConnection_WhenNoMoreOffers()
        {
            IList<IWebElement> elements = new List<IWebElement>();

            System.Collections.ObjectModel.ReadOnlyCollection<IWebElement> enumerableToReutrn
                = new(elements);

            _driverMock.Setup(x => x.FindElements(By.CssSelector("")))
                .Returns(enumerableToReutrn);

            _simpleScrapper.ScrapOfferts(false);

            _driverMock.Verify(x => x.Close(), Times.Once);
        }

        [Fact]
        public void Scrapper_Should_SendRabbitEvent_WhenScrappedOffer()
        {
            Mock<IWebElement> webElementMock = new();

            IList<IWebElement> emptyElements = new List<IWebElement>();

            IList<IWebElement> noEmptyElements = new List<IWebElement>()
            {
               webElementMock.Object,
               webElementMock.Object,
            };

            System.Collections.ObjectModel.ReadOnlyCollection<IWebElement> enumerableToReutrn
                = new(emptyElements);

            System.Collections.ObjectModel.ReadOnlyCollection<IWebElement> notEmptyEnumerableToReutrn
             = new(noEmptyElements);

            int callCount = 0;

            _driverMock.Setup(x => x.FindElements(By.CssSelector("")))
                .Returns(() =>
                {
                    callCount++;
                    return callCount == 1 ? notEmptyEnumerableToReutrn : notEmptyEnumerableToReutrn;
                });

            webElementMock.Setup(x => x.FindElement(By.CssSelector(""))).Returns(webElementMock.Object);

            webElementMock.Setup(x => x.GetAttribute("href")).Returns("coolhref");

            webElementMock.Setup(x => x.Text).Returns("cooltext");

            _driverMock.Setup(x => x.FindElement(By.CssSelector(""))).Returns(webElementMock.Object);

            _simpleScrapper.ScrapOfferts(false);

            _rabbitMessageProducerMock.Verify(x => x.SendMessage(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<JobOfferRaw>()));
        }


        [Fact]
        public void Scrapper_Should_ScrappOnlyTenPages_WhenItIsNotInitScrapp()
        {
            Mock<IWebElement> webElementMock = new();

            IList<IWebElement> emptyElements = new List<IWebElement>();

            IList<IWebElement> noEmptyElements = new List<IWebElement>()
            {
               webElementMock.Object,
            };

            System.Collections.ObjectModel.ReadOnlyCollection<IWebElement> enumerableToReutrn
                = new(emptyElements);

            System.Collections.ObjectModel.ReadOnlyCollection<IWebElement> notEmptyEnumerableToReutrn
             = new(noEmptyElements);

            _driverMock.Setup(x => x.FindElements(By.CssSelector("")))
                .Returns(notEmptyEnumerableToReutrn);

            webElementMock.Setup(x => x.FindElement(By.CssSelector(""))).Returns(webElementMock.Object);

            webElementMock.Setup(x => x.GetAttribute("href")).Returns("coolhref");

            webElementMock.Setup(x => x.Text).Returns("cooltext");

            _driverMock.Setup(x => x.FindElement(By.CssSelector(""))).Returns(webElementMock.Object);

            _simpleScrapper.ScrapOfferts(false);

            // To calculate total page scrapped we can use pattern (calculate find elements method calls) eg.
            // total pages = itemsOnPage / 3 - This times find elements method should be call

            int expectedTimesCallForOneItemOnPageAndPageFromOneTo10 = 10 * 3;

            _driverMock.Verify(x => x.FindElements(It.IsAny<By>()),
                Times.Exactly(expectedTimesCallForOneItemOnPageAndPageFromOneTo10));
        }

        [Theory]
        [InlineData(5)]
        [InlineData(15)]
        [InlineData(34)]
        [InlineData(64)]

        public void Scrapper_Should_ScrappMaxPageCounts_WhenItIsInitScrapp(int totalPagesNumber)
        {

            Mock<IWebElement> webElementMock = new();

            IList<IWebElement> emptyElements = new List<IWebElement>();

            IList<IWebElement> noEmptyElements = new List<IWebElement>()
            {
               webElementMock.Object,
            };

            System.Collections.ObjectModel.ReadOnlyCollection<IWebElement> enumerableToReutrn
                = new(emptyElements);

            System.Collections.ObjectModel.ReadOnlyCollection<IWebElement> notEmptyEnumerableToReutrn
             = new(noEmptyElements);

            int callCount = 0;

            int expectedTimesCall = totalPagesNumber * 3;

            _driverMock.Setup(x => x.FindElements(By.CssSelector("")))
                  .Returns(() =>
                  {
                      callCount++;
                      return callCount <= expectedTimesCall ? notEmptyEnumerableToReutrn : enumerableToReutrn;
                  });

            webElementMock.Setup(x => x.FindElement(By.CssSelector(""))).Returns(webElementMock.Object);

            webElementMock.Setup(x => x.GetAttribute("href")).Returns("coolhref");

            webElementMock.Setup(x => x.Text).Returns("cooltext");

            _driverMock.Setup(x => x.FindElement(By.CssSelector(""))).Returns(webElementMock.Object);

            _simpleScrapper.ScrapOfferts(true);

            // To calculate total page scrapped we can use pattern (calculate find elements method calls) eg.
            // total pages = itemsOnPage / 3 - This times find elements method should be call

            //In this case we setup driver to simulate finding one element on 30 pages

            _driverMock.Verify(x => x.FindElements(It.IsAny<By>()),
                Times.Exactly(expectedTimesCall + 1));

            // + 1 because check call that returns empty result (it ends loop)
        }
    }
}
