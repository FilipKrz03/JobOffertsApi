﻿using AutoMapper;
using JobOffersApiCore.Common;
using JobOffersApiCore.Exceptions;
using JobOffersService.Dto;
using JobOffersService.Entities;
using JobOffersService.Interfaces;
using JobOffersService.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;


namespace JobOfferServiceTests.Services
{
    public class JobOfferServiceTests
    {

        private readonly Mock<IJobOfferRepository> _jobOfferRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<ILogger<JobOffersService.Services.JobOfferService>> _loggerMock;
        private readonly JobOffersService.Services.JobOfferService _offerService;

        public JobOfferServiceTests()
        {
            _mapperMock = new();
            _jobOfferRepositoryMock = new();
            _loggerMock = new();

            _offerService = new(
                _jobOfferRepositoryMock.Object,
                _mapperMock.Object , 
                _loggerMock.Object
                );
        }

        [Fact]
        public async Task Service_GetJobDetail_Should_ThrowResourceNotFoundException_WhenJobOfferNotFound()
        {
            _jobOfferRepositoryMock.Setup(x => x.GetJobOfferWithTechnologies(It.IsAny<Guid>()))
                .ReturnsAsync((JobOffer)null!);

            async Task testCode() => await _offerService.GetJobOfferDetailAsync(It.IsAny<Guid>());

            await Assert.ThrowsAsync<ResourceNotFoundException>(testCode);

            Assert.True(true);
        }

        [Fact]
        public async Task
            Service_GetJobDetail_Should_ReturnNonErrorResponseObjectWithValueOfTypeJobOfferDetail_WhenJobOfferFound()
        {
            _jobOfferRepositoryMock.Setup(x => x.GetJobOfferWithTechnologies(It.IsAny<Guid>()))
                .ReturnsAsync(new JobOffer());

            _mapperMock.Setup(x => x.Map<JobOfferDetailResponse>(It.IsAny<JobOffer>()))
                .Returns(new JobOfferDetailResponse());

            var response = await _offerService.GetJobOfferDetailAsync(It.IsAny<Guid>());

            Assert.IsType<JobOfferDetailResponse>(response);
        }

        [Fact]
        public async Task Service_GetJobOffers_Should_ReturnNonErrorResponseObjectWithValueOfIEnumerableOfJobOfferBasicResponse()
        {
            // Empty IEnumerable<JobOfferBasicResponse> is also proper return type

            _jobOfferRepositoryMock.Setup(x => x.GetJobOffersAsync(It.IsAny<ResourceParamethers>(),
                It.IsAny<Expression<Func<JobOffer, object>>>()))
                .ReturnsAsync(new PagedList<JobOffer>(new List<JobOffer>() , 30 , 1 , 0));

            _mapperMock.Setup(x => x.Map<PagedList<JobOfferBasicResponse>>
            (It.IsAny<PagedList<JobOffer>>()))
                .Returns(new PagedList<JobOfferBasicResponse>(new List<JobOfferBasicResponse>(), 1, 1, 1));
                
            ResourceParamethers resourceParamethers = new();

            var response = await _offerService.GetJobOffersAsync(resourceParamethers);

            Assert.IsAssignableFrom<PagedList<JobOfferBasicResponse>>(response);
        }
    }
}
