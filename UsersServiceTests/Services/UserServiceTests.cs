using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UsersService.Interfaces.RepositoriesInterfaces;
using UsersService.Interfaces.ServicesInterfaces;
using UsersService.Services;
using UsersService.Entities;
using FluentAssertions;
using JobOffersApiCore.Enum;
using UsersService.Exceptions;

namespace UsersServiceTests.Services
{
    public class UserServiceTests
    {

        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IClaimService> _claimServiceMock;
        private readonly UserService _userService;

        public UserServiceTests()
        {
            _userRepositoryMock = new();

            _claimServiceMock = new();

            // Setup to avoid errors (we are not testing logic of this class here)
            _claimServiceMock.Setup(x => x.GetUserIdFromTokenClaim())
                .Returns(Guid.Empty);

            _userService = new(
                _userRepositoryMock.Object,
                _claimServiceMock.Object
                );
        }

        [Fact]
        public async Task Service_UpdateUserSeniority_Should_ThrowInvalidAccesTokenException_WhenUserFromRepositoryEqualsNull()
        {
            _userRepositoryMock.Setup(x => x.GetById(It.IsAny<Guid>()))
                .ReturnsAsync((User)null!);

            await _userService.Invoking(x => x.UpdateUserSeniority(Seniority.Unknown))
                .Should()
                .ThrowAsync<InvalidAccesTokenException>();
        }

        [Fact]
        public async Task Service_UpdateUserSeniority_ShouldNot_ThrowInvalidAccesTokenException_WhenUserFromRepositoryIsNotNull()
        {
            _userRepositoryMock.Setup(x => x.GetById(It.IsAny<Guid>()))
                .ReturnsAsync(new User());

            await _userService.Invoking(x => x.UpdateUserSeniority(Seniority.Unknown))
                .Should()
                .NotThrowAsync<InvalidAccesTokenException>();
        }

        [Fact]
        public async Task Service_UpdateUserSeniority_Should_SaveChangesIntoRepository_WhenUserFromRepositoryIsNotNull()
        {
            _userRepositoryMock.Setup(x => x.GetById(It.IsAny<Guid>()))
                .ReturnsAsync(new User());

            await _userService.UpdateUserSeniority(Seniority.Unknown);

            _userRepositoryMock.Verify(x => x.SaveChangesAsync(), Times.Once);
        }
    }
}
