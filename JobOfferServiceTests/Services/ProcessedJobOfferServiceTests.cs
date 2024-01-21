using AutoMapper;
using Castle.Core.Logging;
using JobOffersApiCore.Dto;
using JobOffersApiCore.Enum;
using JobOffersService.Entities;
using JobOffersService.Interfaces;
using JobOffersService.Services;
using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobOfferServiceTests.Services
{
    public class ProcessedJobOfferServiceTests
    {
        private readonly Mock<ITechnologyRepository> _technologyRepositoryMock;
        private readonly Mock<IJobOfferRepository> _jobOfferRepositoryMock;
        private readonly Mock<ILogger<ProcessedJobOfferService>> _loggerMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly ProcessedJobOfferService _processedJobOfferService;
        private readonly JobOfferProcessed _procesedJobOfferSimple;

        public ProcessedJobOfferServiceTests()
        {
            _technologyRepositoryMock = new();
            _jobOfferRepositoryMock = new();
            _mapperMock = new();
            _loggerMock = new();
            _processedJobOfferService = new(_technologyRepositoryMock.Object, _loggerMock.Object,
                _mapperMock.Object, _jobOfferRepositoryMock.Object);

            _procesedJobOfferSimple = new(
                Guid.Empty ,"", "", "", "",
                Enumerable.Empty<string>(), 
                "", Seniority.Unknown, null, null 
                );
        }

        [Fact]
        public async Task Service_ShouldNot_CallGetAllTechnologiesToRepository_When_DeserializationFailed()
        {
            await _processedJobOfferService.HandleProcessedOfferAsync("Not serializable offer");

            _technologyRepositoryMock.Verify(x => x.GetAllTechnologiesAsync(), Times.Never());
        }

        [Fact]
        public async Task Service_Should_LogError_When_DeserializationFailed()
        {
            await _processedJobOfferService.HandleProcessedOfferAsync("Not serializable offer");

            _loggerMock.Verify(logger => logger.Log(
                It.Is<LogLevel>(logLevel => logLevel == LogLevel.Error),
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => true),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()!),
                Times.Once);
        }

        [Fact]
        public async Task Service_ShouldNot_AddJobOfferToDatabase_When_DeserializationFailed()
        {
            await _processedJobOfferService.HandleProcessedOfferAsync(JsonConvert.SerializeObject("Not deserializable object"));

            _jobOfferRepositoryMock.Verify(x => x.Insert(It.IsAny<JobOffer>()), Times.Never);
            _jobOfferRepositoryMock.Verify(x => x.SaveChangesAsync(), Times.Never);

        }

        [Fact]
        public async Task Service_Should_CallGetAllTechnologiesToRepository_When_DeserializationSucceed()
        {
            await _processedJobOfferService.HandleProcessedOfferAsync(JsonConvert.SerializeObject(_procesedJobOfferSimple));

            _technologyRepositoryMock.Verify(x => x.GetAllTechnologiesAsync(), Times.Once);
        }

        [Fact]
        public async Task Service_Should_MapNewTechnologiesToTechnologiesEntities_When_DeserializationSucceed()
        {
            await _processedJobOfferService.HandleProcessedOfferAsync(JsonConvert.SerializeObject(_procesedJobOfferSimple));

            _mapperMock.Verify(m => m.Map<IEnumerable<Technology>>(It.IsAny<IEnumerable<string>>()) , Times.Once);
        }

        [Fact]
        public async Task Service_Should_AddTechnologieEntitiesToDatabase_WhenDeserializationSucceed()
        {
            await _processedJobOfferService.HandleProcessedOfferAsync(JsonConvert.SerializeObject(_procesedJobOfferSimple));

            _technologyRepositoryMock.Verify(x => x.AddRange(It.IsAny<IEnumerable<Technology>>()), Times.Once);
            _technologyRepositoryMock.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task Service_Should_MapProcessedJobOfferToJobOfferEntite_WhenDeserializationSucceed()
        {
            await _processedJobOfferService.HandleProcessedOfferAsync(JsonConvert.SerializeObject(_procesedJobOfferSimple));

            _mapperMock.Verify(x => x.Map<JobOffer>(It.IsAny<JobOfferProcessed>()) , Times.Once);
        }

        [Fact]
        public async Task Service_Should_CallGetEntitiesFromTechnologyNamesToRepository_When_Deserialization_Succeed()
        {
            await _processedJobOfferService.HandleProcessedOfferAsync(JsonConvert.SerializeObject(_procesedJobOfferSimple));

            _technologyRepositoryMock.Verify(x => x.GetEntitiesFromTechnologiesNamesAsync(It.IsAny<IEnumerable<string>>()), Times.Once);
        }

        [Fact]
        public async Task Service_Should_AddJobOfferToDatabase_When_DeserializationSucceed()
        {
            _mapperMock.Setup(x => x.Map<JobOffer>(It.IsAny<JobOfferProcessed>()))
                .Returns(new JobOffer());

            await _processedJobOfferService.HandleProcessedOfferAsync(JsonConvert.SerializeObject(_procesedJobOfferSimple));

            _jobOfferRepositoryMock.Verify(x => x.Insert(It.IsAny<JobOffer>()), Times.Once);
            _jobOfferRepositoryMock.Verify(x => x.SaveChangesAsync(), Times.Once);
        }
    }
}
