using AutoMapper;
using JobOffersApiCore.Common;
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
        private readonly JobOffersService.Services.JobOfferService _offerService;

        public JobOfferServiceTests()
        {
            _mapperMock = new();
            _jobOfferRepositoryMock = new();
            _offerService = new(_jobOfferRepositoryMock.Object , _mapperMock.Object);
        }

        [Fact]
        public async Task Service_GetJobDetail_Should_Return404ErrorResponseObject_WhenJobOfferNotFound()
        {
            _jobOfferRepositoryMock.Setup(x => x.GetJobOfferWithTechnologies(It.IsAny<Guid>()))
                .ReturnsAsync((JobOffer)null!);

           var response =  await _offerService.GetJobOfferDetail(It.IsAny<Guid>());

           Assert.True(response.ErrorInfo.IsError);
           Assert.Equal(404, response.ErrorInfo.StatusCode);
        }

        [Fact]
        public async Task 
            Service_GetJobDetail_Should_ReturnNonErrorResponseObjectWithValueOfTypeJobOfferDetail_WhenJobOfferFound()
        {
            _jobOfferRepositoryMock.Setup(x => x.GetJobOfferWithTechnologies(It.IsAny<Guid>()))
                .ReturnsAsync(new JobOffer());

            _mapperMock.Setup(x => x.Map<JobOfferDetailResponse>(It.IsAny<JobOffer>()))
                .Returns(new JobOfferDetailResponse());

            var response = await _offerService.GetJobOfferDetail(It.IsAny<Guid>());

            Assert.False(response.ErrorInfo.IsError);
            Assert.IsType<JobOfferDetailResponse>(response.Value);
        }

        [Fact]
        public async Task Service_GetJobOffers_Should_ReturnNonErrorResponseObjectWithValueOfIEnumerableOfJobOfferBasicResponse()
        {
            // Empty IEnumerable<JobOfferBasicResponse> is also proper return type

            _jobOfferRepositoryMock.Setup(x => x.GetJobOffersAsync(It.IsAny<ResourceParamethers>() , 
                It.IsAny<Expression<Func<JobOffer,object>>>()))
                .ReturnsAsync(Enumerable.Empty<JobOffer>());

            _mapperMock.Setup(x => x.Map<IEnumerable<JobOfferBasicResponse>>(It.IsAny<IEnumerable<JobOfferBasicResponse>>()));

            ResourceParamethers resourceParamethers = new();
            
            var response = await _offerService.GetJobOffers(resourceParamethers);

            Assert.False(response.ErrorInfo.IsError);
            Assert.IsAssignableFrom<IEnumerable<JobOfferBasicResponse>>(response.Value);
        }
    }
}
