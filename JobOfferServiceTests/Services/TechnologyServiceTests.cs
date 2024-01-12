using AutoMapper;
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
    public class TechnologyServiceTests
    {
        private readonly Mock<ITechnologyRepository> _technologyRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly TechnologyService _technologyService;

        public TechnologyServiceTests()
        {
            _technologyRepositoryMock = new();
            _mapperMock = new();
            _technologyService = new(_technologyRepositoryMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task 
            Service_GetTechnologies_Should_ReturnNonErrorResponseObjectWithValueOfIEnumerableOfTechnologyBasicResponse()
        {
            _technologyRepositoryMock.Setup(x => x.GetTechnologiesAsync(It.IsAny<ResourceParamethers>(),
                It.IsAny<Expression<Func<Technology, object>>>()))
                .ReturnsAsync(new PagedList<Technology>(new List<Technology>() , 30 , 1 , 0));

            _mapperMock.Setup(x => x.Map<PagedList<TechnologyBasicResponse>>
            (It.IsAny<PagedList<Technology>>()))
                .Returns(new PagedList<TechnologyBasicResponse>(new List<TechnologyBasicResponse>(), 1, 1, 1));

            var response = await _technologyService.GetTechnologies(new ResourceParamethers());

            Assert.IsAssignableFrom<PagedList<TechnologyBasicResponse>>(response);
        }

        [Fact]
        public async Task Service_GetTechnologyWithJobOffers_Should_ThrowResourceNotFoundException_WhenTechnologyNotFound()
        {
            _technologyRepositoryMock.Setup(x => x.GetTechnologyWithJobOffersAsync
            (It.IsAny<Guid>(), It.IsAny<ResourceParamethers>()))
                .ReturnsAsync((Technology)null!);

            async Task testCode() => await _technologyService.
                GetTechnologyWithJobOffers(Guid.Empty, new ResourceParamethers());

            await Assert.ThrowsAsync<ResourceNotFoundException>(testCode);
        }

        [Fact]
        public async Task 
            Service_GetTechnologyWithJobOffers_Should_ReturnNonErrorResponseObjectWithValuePropertyOfTechnologyDetailResponse_WhenTechnologyFound()
        {
            _technologyRepositoryMock.Setup(x => x.GetTechnologyWithJobOffersAsync
            (It.IsAny<Guid>(), It.IsAny<ResourceParamethers>()))
                .ReturnsAsync(new Technology());

            _mapperMock.Setup(x => x.Map<TechnologyDetailResponse>(It.IsAny<Technology>()))
                .Returns(new TechnologyDetailResponse(Guid.Empty , "" , Enumerable.Empty<JobOfferBasicResponse>()));

            var repsonse = await _technologyService.
                GetTechnologyWithJobOffers(Guid.Empty, new ResourceParamethers());

            Assert.IsType<TechnologyDetailResponse>(repsonse);
        }
    }
}
