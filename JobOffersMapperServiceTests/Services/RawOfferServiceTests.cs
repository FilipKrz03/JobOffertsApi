using AutoMapper;
using JobOffersApiCore.Dto;
using JobOffersApiCore.Interfaces;
using JobOffersMapperService.Interfaces;
using JobOffersMapperService.Services;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobOffersMapperServiceTests.Services
{
    public class RawOfferServiceTests
    {
        private Mock<IOffersBaseRepository> _offersBaseRepositoryMock;
        private Mock<IMapper> _mapperMock;
        private Mock<ILogger<RawOfferService>> _loggerMock;
        private Mock<IRabbitMessageProducer> _rabbitMessageProducerMock;

        public RawOfferServiceTests()
        {
            _offersBaseRepositoryMock = new();
            _mapperMock = new();
            _loggerMock = new();
            _rabbitMessageProducerMock = new();
        }

        [Fact]
        public async Task Service_ShouldNot_CheckIfOfferExist_WhenDeserializationFailed()
        {
            var rawOfferService = new RawOfferService(_offersBaseRepositoryMock.Object,
                _mapperMock.Object, _loggerMock.Object, _rabbitMessageProducerMock.Object);

            await rawOfferService.HandleRawOffer("bad object");

            _offersBaseRepositoryMock.Verify(r => r.OfferExistAsync(It.IsAny<JobOfferRaw>()), Times.Never);
        }
    }
}
