using AutoMapper;
using JobOffersApiCore.Dto;
using JobOffersApiCore.Interfaces;
using JobOffersMapperService.Entites;
using JobOffersMapperService.Interfaces;
using JobOffersMapperService.Services;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobOffersMapperServiceTests.Services
{
    public class RawOfferServiceTests
    {
        private readonly Mock<IOffersBaseRepository> _offersBaseRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<ILogger<RawOfferService>> _loggerMock;
        private readonly Mock<IRabbitMessageProducer> _rabbitMessageProducerMock;
        private readonly JobOfferRaw _simpleJobOfferRaw;
        private readonly RawOfferService _rawOfferService;

        public RawOfferServiceTests()
        {
            _offersBaseRepositoryMock = new();
            _mapperMock = new();
            _loggerMock = new();
            _rabbitMessageProducerMock = new();
            _simpleJobOfferRaw = new("", "", "", "", "", Enumerable.Empty<string>(), "", "");
            _rawOfferService = new RawOfferService(_offersBaseRepositoryMock.Object,
           _mapperMock.Object, _loggerMock.Object, _rabbitMessageProducerMock.Object);
        }

        [Fact]
        public async Task Service_ShouldNot_CheckIfOfferExist_WhenDeserializationFailed()
        {
            await _rawOfferService.HandleRawOffer("bad object");

            _offersBaseRepositoryMock.Verify(r => r.OfferExistAsync(It.IsAny<JobOfferRaw>()), Times.Never);
        }

        [Fact]
        public async Task Service_Should_CheckIfOfferExist_WhenDeserializationSucceded()
        {
            await _rawOfferService.HandleRawOffer(JsonConvert.SerializeObject(_simpleJobOfferRaw));

            _offersBaseRepositoryMock.Verify(r => r.OfferExistAsync(It.IsAny<JobOfferRaw>()), Times.Once);
        }

        [Fact]
        public async Task Service_ShouldNot_MapJobOffer_WhenJobOfferAlreadyExist()
        {
            SetupOfferBaseRepositoryMockOfferExistAsyncMethod(true);

            await _rawOfferService.HandleRawOffer(JsonConvert.SerializeObject(_simpleJobOfferRaw));

            _mapperMock.Verify(m => m.Map<JobOfferRaw , JobOfferBase>(It.IsAny<JobOfferRaw>()), Times.Never);
        }

        [Fact]
        public async Task Service_Should_MapToJobOfferBase_WhenJobOfferDoesNotExistInDatabase()
        {
            SetupOfferBaseRepositoryMockOfferExistAsyncMethod(false);

            await _rawOfferService.HandleRawOffer(JsonConvert.SerializeObject(_simpleJobOfferRaw));

            _mapperMock.Verify(m => m.Map<JobOfferRaw, JobOfferBase>(It.IsAny<JobOfferRaw>()), Times.Once);
        }

        [Fact]
        public async Task Service_Should_CallInsertJobOfferBaseToRepository_WhenJobOfferDoesNotExistInDatabase()
        {
            SetupOfferBaseRepositoryMockOfferExistAsyncMethod(false);

            await _rawOfferService.HandleRawOffer(JsonConvert.SerializeObject(_simpleJobOfferRaw));

            _offersBaseRepositoryMock.Verify(x => x.Insert(It.IsAny<JobOfferBase>()), Times.Once);
            _offersBaseRepositoryMock.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task Service_ShouldNot_CallInsertJobOfferBaseToRepository_WhenJobOfferExistInDatabase()
        {
            SetupOfferBaseRepositoryMockOfferExistAsyncMethod(true);
            
            await _rawOfferService.HandleRawOffer(JsonConvert.SerializeObject(_simpleJobOfferRaw));

            _offersBaseRepositoryMock.Verify(x => x.Insert(It.IsAny<JobOfferBase>()), Times.Never);
            _offersBaseRepositoryMock.Verify(x => x.SaveChangesAsync(), Times.Never);
        }

        [Fact]
        public async Task Service_Should_MapToJobOfferProcessed_WhenJobOfferDoesNotExistInDatabase()
        {
            SetupOfferBaseRepositoryMockOfferExistAsyncMethod(false);

            await _rawOfferService.HandleRawOffer(JsonConvert.SerializeObject(_simpleJobOfferRaw));

            _mapperMock.Verify(m => m.Map<JobOfferRaw, JobOfferProcessed>(It.IsAny<JobOfferRaw>()), Times.Once);
        }

        [Fact]
        public async Task Service_Should_SendRabbitEventWithJobOfferProcessed_WhenJobOfferDoesNotExistInDatabase()
        {
            SetupOfferBaseRepositoryMockOfferExistAsyncMethod(false);

            await _rawOfferService.HandleRawOffer(JsonConvert.SerializeObject(_simpleJobOfferRaw));

            _rabbitMessageProducerMock.Verify(x => x.SendMessage
            (It.IsAny<string>(), It.IsAny<string>(), It.IsAny<JobOfferProcessed>()), Times.Once);
        }

        [Fact]
        public async Task Service_ShouldNot_SendRabbitEvent_WhenJobOfferExistInDatabase()
        {
            SetupOfferBaseRepositoryMockOfferExistAsyncMethod(true);

            await _rawOfferService.HandleRawOffer(JsonConvert.SerializeObject(_simpleJobOfferRaw));

            _rabbitMessageProducerMock.Verify(x => x.SendMessage
            (It.IsAny<string>(), It.IsAny<string>(), It.IsAny<JobOfferProcessed>()), Times.Never);
        }

        private void SetupOfferBaseRepositoryMockOfferExistAsyncMethod(bool exist)
        {
            _offersBaseRepositoryMock.Setup(x => x.OfferExistAsync(It.IsAny<JobOfferRaw>()))
             .ReturnsAsync(exist);
        }
    }
}
