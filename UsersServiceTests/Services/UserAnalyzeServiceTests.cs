using Bogus;
using JobOffersApiCore.Enum;
using JobOffersApiCore.Interfaces;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UsersService.Dto;
using UsersService.Entities;
using UsersService.Interfaces.RepositoriesInterfaces;
using UsersService.Interfaces.ServicesInterfaces;
using UsersService.Services;

namespace UsersServiceTests.Services
{
    public class UserAnalyzeServiceTests
    {

        private readonly Mock<IJobOfferRepository> _jobOfferRepositoryMock;
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IRabbitMessageProducer> _rabbitMessageProducerMock;
        private readonly Mock<IMailContentCreatorService> _mailContentCreatorServiceMock;

        private readonly UserAnalyzeService _userAnalyzeService;

        public UserAnalyzeServiceTests()
        {
            _jobOfferRepositoryMock = new();
            _userRepositoryMock = new();
            _rabbitMessageProducerMock = new();
            _mailContentCreatorServiceMock = new();

            _userAnalyzeService = new(
                _jobOfferRepositoryMock.Object,
                _userRepositoryMock.Object,
                _rabbitMessageProducerMock.Object,
                _mailContentCreatorServiceMock.Object
                );
        }

        [Fact]
        public async Task Service_LetUsersKnowAboutNewMatchingOffers_ShouldNot_SendAnyMessages_WhenAllUsersIsEmptyList()
        {
            _userRepositoryMock.Setup(x =>
              x.GetAllUsersWithEmailSeniorityAndTechnologiesAsync())
                .ReturnsAsync(Enumerable.Empty<UserWithEmailSeniorityAndTechnolgiesDto>());

            await _userAnalyzeService.LetUsersKnowAboutNewMatchingOffersAsync();

            _rabbitMessageProducerMock.Verify(x =>
              x.SendMessage(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<object>()), Times.Never);
        }

        [Fact]
        public async Task Service_LetUsersKnowAboutNewMatchingOffersAsync_Should_SendOneMessage_WhenOnlyUsersWithoutSubscribedTechnologiesExists()
       
        {
            var usersWithNoSubscribedTechFake = new Faker<UserWithEmailSeniorityAndTechnolgiesDto>()
                 .CustomInstantiator(f =>
                    new UserWithEmailSeniorityAndTechnolgiesDto(
                       f.Internet.Email(),
                       Seniority.Unknown,
                       new List<Technology>()));

            List<UserWithEmailSeniorityAndTechnolgiesDto> listOfUserWithNoSubscribedTech
                = usersWithNoSubscribedTechFake.Generate(5);

            _userRepositoryMock.Setup(x =>
              x.GetAllUsersWithEmailSeniorityAndTechnologiesAsync())
                .ReturnsAsync(listOfUserWithNoSubscribedTech);

            await _userAnalyzeService.LetUsersKnowAboutNewMatchingOffersAsync();

            _rabbitMessageProducerMock.Verify(x =>
              x.SendMessage(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<object>()), Times.Once);
        }

        [Fact]
        public async Task
            Service_LetUsersKnowAboutNewMatchingOffersAsync_Should_SendTwoMessages_WhenUsersWithoutSubscribedTechnologiesExistsAndUsersWithSubscribedTechnologiesExists()

        {
            var usersWithNoSubscribedTechFake = new Faker<UserWithEmailSeniorityAndTechnolgiesDto>()
                 .CustomInstantiator(f =>
                    new UserWithEmailSeniorityAndTechnolgiesDto(
                       f.Internet.Email(),
                       Seniority.Unknown,
                       new List<Technology>()));

            List<UserWithEmailSeniorityAndTechnolgiesDto> listOfUsers
                = usersWithNoSubscribedTechFake.Generate(5);

            listOfUsers.Add(new UserWithEmailSeniorityAndTechnolgiesDto(
                "",
                Seniority.Unknown,
                new List<Technology>()
                {
                    new Technology()
                }
                ));

            _userRepositoryMock.Setup(x =>
              x.GetAllUsersWithEmailSeniorityAndTechnologiesAsync())
                .ReturnsAsync(listOfUsers);

            await _userAnalyzeService.LetUsersKnowAboutNewMatchingOffersAsync();

            _rabbitMessageProducerMock.Verify(x =>
              x.SendMessage(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<object>()), Times.Exactly(2));
        }
    }
}
