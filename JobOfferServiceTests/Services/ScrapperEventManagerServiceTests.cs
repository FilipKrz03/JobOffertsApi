using JobOffersApiCore.Interfaces;
using JobOfferService.Props;
using JobOffersService.Interfaces;
using JobOffersService.Jobs;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static JobOfferService.Props.RabbitMQOffersScraperProps;

namespace JobOfferServiceTests.Services
{
    public class ScrapperEventManagerServiceTests
    {
        private readonly Mock<IRabbitMessageProducer> _rabbitMessageProducerMock;
        private readonly ScrapperEventManagerJob _scrapperEventManagerJob;
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

            _scrapperEventManagerJob = new
                (_rabbitMessageProducerMock.Object,
                serviceProviderMock.Object
                );
        }

        [Theory]
        [InlineData(false, OFFERS_CREATE_MESSAGE)]
        [InlineData(true, OFFERS_UPDATE_MESSAGE)]
        public async Task
            Service_Should_SendProperRabbitEventMessage_DependingOnDatabaseInitialization(bool isDbInitalized, string message)
        {
            _jobOfferRepositoryMock.Setup(r => r.IsDatabaseInitalizedAsync())
               .ReturnsAsync(isDbInitalized);

            await _scrapperEventManagerJob.Execute(default!);

            _rabbitMessageProducerMock.Verify
                (x => x.SendMessage(It.IsAny<string>(), It.IsAny<string>(), message));
        }
    }
}
