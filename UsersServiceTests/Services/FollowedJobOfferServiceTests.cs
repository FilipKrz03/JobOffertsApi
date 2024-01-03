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
using UsersService.Interfaces;
using UsersService.Services;

namespace UsersServiceTests.Services
{
    public class FollowedJobOfferServiceTests
    {

        private readonly Mock<IUserRepository> _userReposiotryMock;
        private readonly Mock<IJobOfferRepository> _jobOfferRepositoryMock;
        private readonly Mock<IClaimService> _claimServiceMock;
        private readonly Mock<IJobOfferUserJoinRepository> _jobOfferUserJoinRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly FollowedJobOfferService _followedJobOfferService;


        public FollowedJobOfferServiceTests()
        {
            _userReposiotryMock = new();
            _jobOfferRepositoryMock = new();
            _claimServiceMock = new();
            _jobOfferUserJoinRepositoryMock = new();
            _mapperMock = new();

            // Setup to avoid errors before each test (we are not testing claim service logic here)
            _claimServiceMock.Setup(x => x.GetUserIdFromTokenClaim())
                .Returns(Guid.NewGuid());

            _followedJobOfferService = new(_userReposiotryMock.Object, _jobOfferRepositoryMock.Object ,
                _claimServiceMock.Object, _jobOfferUserJoinRepositoryMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task Service_AddFollowedJobOffer_Should_ThrowInvalidAccesTokenException_WhenUserNotFound()
        {
            _userReposiotryMock.Setup(x => x.GetById(It.IsAny<Guid>()))
                .ReturnsAsync((User)null!);

            await _followedJobOfferService.Invoking(x => x.AddFolowedJobOffer(Guid.NewGuid()))
                .Should()
                .ThrowAsync<InvalidAccesTokenException>();
        }

        [Fact]
        public async Task Service_AddFollowedJobOffer_Should_ThrowResourceNotFoundException_WhenJobOfferToAddNotFound()
        {
            // Setup to reach testing logic
            _userReposiotryMock.Setup(x => x.GetById(It.IsAny<Guid>()))
                .ReturnsAsync(new User());
            //

            _jobOfferRepositoryMock.Setup(x => x.GetById(It.IsAny<Guid>()))
                .ReturnsAsync((JobOffer)null!);

            await _followedJobOfferService.Invoking(x => x.AddFolowedJobOffer(Guid.NewGuid()))
                .Should()
                .ThrowAsync<ResourceNotFoundException>();
        }

        [Fact]
        public async Task Service_AddFollowedJobOffer_Should_ThrowResourceAlreadyExistException_WhenFoollowedJobOfferAlreadyExist()
        {
            // Setup to reach testing logic
            _userReposiotryMock.Setup(x => x.GetById(It.IsAny<Guid>()))
               .ReturnsAsync(new User());

            _jobOfferRepositoryMock.Setup(x => x.GetById(It.IsAny<Guid>()))
                .ReturnsAsync(new JobOffer());
            //

            _jobOfferUserJoinRepositoryMock.Setup(x => x.UserJobOfferExistAsync(It.IsAny<Guid>(), It.IsAny<Guid>()))
                .ReturnsAsync(true);


            await _followedJobOfferService.Invoking(x => x.AddFolowedJobOffer(Guid.NewGuid()))
                .Should()
                .ThrowAsync<ResourceAlreadyExistException>();
        }

        [Fact]
        public async Task Service_AddFollowedJobOffer_Should_InsertJobOfferToFollowedUserOffers_WhenNoErrosThrown()
        {
            string offerCompany = "CoolCompany123";
            string localization = "CoolLocalization";
            JobOffer offerToAdd = new() { OfferCompany = offerCompany, Localization = localization };

            User userToAddOfferTo = new();

            _userReposiotryMock.Setup(x => x.GetById(It.IsAny<Guid>()))
               .ReturnsAsync(userToAddOfferTo);

            _jobOfferRepositoryMock.Setup(x => x.GetById(It.IsAny<Guid>()))
                .ReturnsAsync(offerToAdd);
        
            _jobOfferUserJoinRepositoryMock.Setup(x => x.UserJobOfferExistAsync(It.IsAny<Guid>(), It.IsAny<Guid>()))
                .ReturnsAsync(false);

            await _followedJobOfferService.AddFolowedJobOffer(Guid.NewGuid());

            userToAddOfferTo.JobOffers.Last().Should().Be(offerToAdd);
            _userReposiotryMock.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task Service_DeleteFollowedJobOffer_Should_ThrowResourceNotFoundException_WhenJobOfferUserJoinRecordNotFound()
        {
            _jobOfferUserJoinRepositoryMock.Setup
                (x => x.GetUserJobOfferJoinAsync(It.IsAny<Guid>(), It.IsAny<Guid>()))
                .ReturnsAsync((JobOfferUser)null!);

            await _followedJobOfferService.Invoking(x => x.DeleteFollowedJobOffer(Guid.NewGuid()))
                .Should()
                .ThrowAsync<ResourceNotFoundException>();
        }

        [Fact]  
        public async Task Service_DeleteFollowedJobOffer_Should_JobOfferUserJoin_WhenJobOfferUserJoinRecordFound()
        {
            _jobOfferUserJoinRepositoryMock.Setup
                (x => x.GetUserJobOfferJoinAsync(It.IsAny<Guid>(), It.IsAny<Guid>()))
                .ReturnsAsync(new JobOfferUser());

            await _followedJobOfferService.DeleteFollowedJobOffer(Guid.NewGuid());

            _jobOfferUserJoinRepositoryMock.Verify
             (x => x.RemoveUserJobOffer(It.IsAny<JobOfferUser>()), Times.Once);

            _jobOfferUserJoinRepositoryMock.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task Service_GetFollowedJobOffer_Should_ThrowResourceNotFoundException_WhenUserJobOfferNotFound()
        {
            _jobOfferRepositoryMock.Setup(x => x.GetUserJobOffer(It.IsAny<Guid>(), It.IsAny<Guid>()))
                .ReturnsAsync((JobOffer)null!);

            await _followedJobOfferService.Invoking(x => x.GetFollowedJobOffer(Guid.NewGuid()))
                .Should()
                .ThrowAsync<ResourceNotFoundException>();
        }

        [Fact]
        public async Task Service_GetFollowedJobOffer_Should_ReturnResponseMappedToJobOfferDetailResponseDto_WhenUserJobOfferFound()
        {
            string localization = "CoolLocaliation1111";
            string company = "coolCompany12131";
            Guid offerId = Guid.NewGuid();

            JobOffer offerToMap = new() { Id = offerId, Localization = localization, OfferCompany = company };
            JobOfferDetailResponseDto offertToReturn = new() { Id = offerId , Localization = localization , OfferCompany = company };

            _jobOfferRepositoryMock.Setup(x => x.GetUserJobOffer(It.IsAny<Guid>(), It.IsAny<Guid>()))
               .ReturnsAsync(offerToMap);

            _mapperMock.Setup(x => x.Map<JobOfferDetailResponseDto>(offerToMap))
                .Returns(offertToReturn);

            var result = await _followedJobOfferService.GetFollowedJobOffer(Guid.NewGuid());

            result!.Id.Should().Be(offerId);
            result!.Localization.Should().Be(localization); 
            result!.OfferCompany.Should().Be(company);

            result.Should().BeOfType<JobOfferDetailResponseDto>();
        }

        [Fact]
        public async Task Service_GetFollowedJobOffers_Should_ReturnPagedListOfJobOfferBasicResponse()
        {
            ResourceParamethers resourceParamethers = new();

            string company = "coolcmp";

            List<JobOfferBasicResponseDto> listToReturn = new()
            {
                new JobOfferBasicResponseDto() { OfferCompany = company }
            };

            var pagedListToReturn = new PagedList<JobOfferBasicResponseDto>(listToReturn, 10, 10, 10);

            _mapperMock.Setup(x => x.Map<PagedList<JobOfferBasicResponseDto>>(It.IsAny<PagedList<JobOffer?>>()))
                .Returns(pagedListToReturn);

            var result = await _followedJobOfferService.GetFollowedJobOffers(resourceParamethers);

            result.Should().BeOfType<PagedList<JobOfferBasicResponseDto>>();
            result[0].OfferCompany.Should().Be(company);
        }
    }
}
