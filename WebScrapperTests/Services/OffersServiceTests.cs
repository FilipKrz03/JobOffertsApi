using Castle.Core.Logging;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebScrapperService.Interfaces;
using WebScrapperService.Props;
using WebScrapperService.Services;
using static WebScrapperService.Props.RabbitMQOffersEventProps;

namespace WebScrapperTests.Services
{
    public class OffersServiceTests
    {

        private readonly OffersService _offersService;
        private readonly Mock<IScrapperService> _scrapperServiceMock1;
        private readonly Mock<IScrapperService> _scrapperServiceMock2;
        private readonly Mock<ILogger<OffersService>> _loggerMock;

        public OffersServiceTests()
        {
            _scrapperServiceMock1 = new();
            _scrapperServiceMock2 = new();
            _loggerMock = new();

            IEnumerable<IScrapperService> scrapperServices = new List<IScrapperService>()
            {
                _scrapperServiceMock1.Object ,
                _scrapperServiceMock2.Object
            };

            _offersService = new(scrapperServices, _loggerMock.Object);
        }

        [Theory]
        [InlineData($"\"{OFFERS_CREATE_MESSAGE}\"", true)]
        [InlineData($"\"{OFFERS_UPDATE_MESSAGE}\"", false)]
        public void Service_Should_ScrappOffersWithProperLogic_WhenMessageRecognized(string routingKey, bool isInitLogic)
        {
            _offersService.HandleOffersCreateAndUpdate(routingKey);

            _scrapperServiceMock1.Verify(x => x.ScrapOfferts(isInitLogic));
            _scrapperServiceMock2.Verify(x => x.ScrapOfferts(isInitLogic));
        }

        [Fact]
        public void Service_ShouldNot_ScrappOffers_WhenMessageNotRecognized()
        {
            _offersService.HandleOffersCreateAndUpdate("\"Not recognized message\"");

            _scrapperServiceMock1.Verify(x => x.ScrapOfferts(It.IsAny<bool>()), Times.Never);
            _scrapperServiceMock2.Verify(x => x.ScrapOfferts(It.IsAny<bool>()), Times.Never);
        }

        [Fact]
        public void Service_Should_ThrowCatchAndLogArgumentException_WhenRoutingKeyNotRecognized()
        {
            _offersService.HandleOffersCreateAndUpdate("\"Not recognized message\"");

            _loggerMock.Verify(logger => logger.Log(
           It.Is<LogLevel>(logLevel => logLevel == LogLevel.Error),
           It.IsAny<EventId>(),
           It.Is<It.IsAnyType>((v, t) => true),
           It.IsAny<ArgumentException>(),
           It.IsAny<Func<It.IsAnyType, Exception, string>>()!),
           Times.Once);
        }
    }
}
