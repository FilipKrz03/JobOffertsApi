using JobOffersApiCore.Interfaces;
using JobOfferService.Props;
using JobOfferService.Services;
using JobOffersService.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobOfferServiceTests.Services
{
    public class ScrapperEventManagerServiceTests
    {
        private readonly Mock<IRabbitMessageProducer> _rabbitMessageProducerMock;
        private readonly ScrapperEventManagerService _scrapperEventManagerService;
        private readonly Mock<IJobOfferRepository> _jobOfferRepositoryMock;

        public ScrapperEventManagerServiceTests()
        {
            _rabbitMessageProducerMock = new();
            _jobOfferRepositoryMock = new();

            var serviceProviderMock = new Mock<IServiceProvider>();
            serviceProviderMock.Setup(x => x.GetService(typeof(IJobOfferRepository)))
                .Returns(_jobOfferRepositoryMock.Object);

            var serviceScope = new Mock<IServiceScope>();
            serviceScope.Setup(x => x.ServiceProvider).Returns(serviceProviderMock.Object);

            var serviceScopeFactory = new Mock<IServiceScopeFactory>();
            serviceScopeFactory.Setup(x => x.CreateScope())
                .Returns(serviceScope.Object);

            serviceProviderMock.Setup(x => x.GetService(typeof(IServiceScopeFactory)))
                .Returns(serviceScopeFactory.Object);

            _scrapperEventManagerService = new(_rabbitMessageProducerMock.Object, serviceProviderMock.Object);
        }

        [Theory]
        [InlineData(false , RabbitMQOffersScraperProps.OFFERS_CREATE_ROUTING_KEY)]
        [InlineData(true , RabbitMQOffersScraperProps.OFFERS_UPDATE_ROUTING_KEY)]
        public async Task 
            Service_Should_SendProperRabbitEvent_DependingOnDatabaseInitialization(bool isDbInitalized, string routingKey)
        {
            _jobOfferRepositoryMock.Setup(r => r.IsDatabaseInitalizedAsync())
               .ReturnsAsync(isDbInitalized);

            await _scrapperEventManagerService.StartAsync(default);

            _rabbitMessageProducerMock.Verify
                (x => x.SendMessage(It.IsAny<string>(), routingKey));
        }
    }
}
