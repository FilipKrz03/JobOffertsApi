using AutoMapper;
using FluentAssertions;
using JobOffersApiCore.Common;
using JobOffersApiCore.Exceptions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UsersService.Dto;
using UsersService.Entities;
using UsersService.Exceptions;
using UsersService.Interfaces.RepositoriesInterfaces;
using UsersService.Interfaces.ServicesInterfaces;
using UsersService.Services;

namespace UsersServiceTests.Services
{
    public class SubscribedTechnologyServiceTests
    {

        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<ITechnologyRepository> _technologyRepositoryMock;
        private readonly Mock<ITechnologyUserJoinRepository> _technologyUserJoinRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly SubscribedTechnologyService _subscribedTechnologyService;

        public SubscribedTechnologyServiceTests()
        {
            _userRepositoryMock = new();
            _technologyRepositoryMock = new();
            _technologyUserJoinRepositoryMock = new();
            _mapperMock = new();

            Mock<IClaimService> claimServiceMock = new();

            // Setup to avoid errors before each test (we are not testing claim service logic here)
            claimServiceMock.Setup(x => x.GetUserIdFromTokenClaim())
                .Returns(Guid.NewGuid());

            _subscribedTechnologyService = new(claimServiceMock.Object, _userRepositoryMock.Object,
                _technologyRepositoryMock.Object, _technologyUserJoinRepositoryMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task Service_AddSubscribedTechnology_Should_ThrowInvalidAccesTokenException_WhenUserNotFound()
        {
            _userRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync((User)null!);

            await _subscribedTechnologyService.Invoking(x => x.AddSubscribedTechnologyAsync(Guid.NewGuid()))
                .Should()
                .ThrowAsync<InvalidAccesTokenException>();
        }

        [Fact]
        public async Task Service_AddSubscribedTechnology_Should_ThrowResourceNotFoundException_WhenTechnologyNotFound()
        {
            _userRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(new User());

            _technologyRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync((Technology)null!);

            await _subscribedTechnologyService.Invoking(x => x.AddSubscribedTechnologyAsync(Guid.NewGuid()))
                .Should()
                .ThrowAsync<ResourceNotFoundException>();
        }

        [Fact]
        public async Task Service_AddSubscribedTechnology_Should_ThrowResourceAlreadyExistException_WhenUserTechnologyJoinEntityAlreadyExist()
        {
            _userRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(new User());

            _technologyRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(new Technology());

            _technologyUserJoinRepositoryMock.Setup(x => x.UserTechnologyExistAsync(It.IsAny<Guid>(), It.IsAny<Guid>()))
                .ReturnsAsync(true);

            await _subscribedTechnologyService.Invoking(x => x.AddSubscribedTechnologyAsync(Guid.NewGuid()))
                .Should()
                .ThrowAsync<ResourceAlreadyExistException>();
        }

        [Fact]
        public async Task Service_AddSubscribedTechnology_Should_AddSubscribedTechnologyToDatabase_WhenNoErrorThrown()
        {
            string technologyName = "hardtech";

            var userToAddTechnologyTo = new User();

            var technologyToAdd = new Technology() { TechnologyName = technologyName };

            _userRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
              .ReturnsAsync(userToAddTechnologyTo);

            _technologyRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(technologyToAdd);

            _technologyUserJoinRepositoryMock.Setup(x => x.UserTechnologyExistAsync(It.IsAny<Guid>(), It.IsAny<Guid>()))
                .ReturnsAsync(false);

            await _subscribedTechnologyService.AddSubscribedTechnologyAsync(Guid.NewGuid());

            userToAddTechnologyTo.Technologies.Last().Should().Be(technologyToAdd);

            _userRepositoryMock.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task Service_DeleteSubscribedTechnology_Should_ThrowResourceNotFoundException_WhenUserTechnologyJoinEntityNotFound()
        {
            _technologyUserJoinRepositoryMock.Setup(x => x.GetTechnologyUserJoinEntitiyAsync
            (It.IsAny<Guid>(), It.IsAny<Guid>()))
                .ReturnsAsync((TechnologyUser)null!);

            await _subscribedTechnologyService.Invoking(x => x.DeleteSubscribedTechnologyAsync(Guid.Empty))
                .Should()
                .ThrowAsync<ResourceNotFoundException>();
        }

        [Fact]
        public async Task Service_DeleteSubscribedTechnology_Should_DeleteJoinTechnologyUserEntitieFromDatabase_WhenEntitieExist()
        {
            _technologyUserJoinRepositoryMock.Setup(x => x.GetTechnologyUserJoinEntitiyAsync
            (It.IsAny<Guid>(), It.IsAny<Guid>()))
                .ReturnsAsync(new TechnologyUser());

            await _subscribedTechnologyService.DeleteSubscribedTechnologyAsync(Guid.Empty);

            _technologyUserJoinRepositoryMock.Verify
                (x => x.DeleteTechnologyUserJoinEntity(It.IsAny<TechnologyUser>()), Times.Once);

            _technologyUserJoinRepositoryMock.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task Service_GetSubscribedTechnologies_Should_ReturnPagedListOfTechnologyBasicResponse()
        {
            string technolgoyName = "coolTechName";

            List<TechnologyBasicResponseDto> technologies = new()
            {
                new TechnologyBasicResponseDto(Guid.Empty , technolgoyName)
            };

            PagedList<TechnologyBasicResponseDto> mappedTechnologies = new(technologies, 10, 10, 10);

            ResourceParamethers resourceParamethers = new();

            _mapperMock.Setup(x => x.Map<PagedList<TechnologyBasicResponseDto>>(It.IsAny<PagedList<Technology>>()))
                .Returns(mappedTechnologies);

            var result = await _subscribedTechnologyService.GetSubscribedTechnologiesAsync(resourceParamethers);

            result.Should().BeOfType<PagedList<TechnologyBasicResponseDto>>();
            result[0].TechnologyName.Should().Be(technolgoyName);
        }
    }
}
