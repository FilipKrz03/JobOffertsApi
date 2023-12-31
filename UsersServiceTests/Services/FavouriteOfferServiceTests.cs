using FluentAssertions;
using JobOffersApiCore.Exceptions;
using Moq;
using RichardSzalay.MockHttp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using UsersService.Entities;
using UsersService.Interfaces;
using UsersService.Services;

namespace UsersServiceTests.Services
{
    public class FavouriteOfferServiceTests
    {

        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IFavouriteOfferRepositroy> _favouriteOfferRepositoryMock;
        private readonly MockHttpMessageHandler _httpMock;

        public FavouriteOfferServiceTests()
        {
            _favouriteOfferRepositoryMock = new();
            _userRepositoryMock = new();
            _httpMock = new();
        }

        [Fact]
        public async Task Service_CreateFavouriteOffer_Should_ThrowResourceAlreadyExistException_WhenUserAlreadyHaveFavouriteOfferWithProvidedId()
        {
            _favouriteOfferRepositoryMock.Setup(x => x.UserFavouriteOfferExist(It.IsAny<Guid>(), It.IsAny<Guid>()))
                .ReturnsAsync(true);

            FavouriteOfferService service = new
                (_userRepositoryMock.Object, _httpMock.ToHttpClient(), _favouriteOfferRepositoryMock.Object);

            await service.Invoking(x => x.CreateFavouriteOffer(Guid.Empty, Guid.Empty))
                .Should()
                .ThrowAsync<ResourceAlreadyExistException>();
        }

        [Fact]
        public async Task Service_CreateFavouriteOffer_Should_ThrowResourceNotFoundException_WhenOfferExistRequestResponedWithNotSuceedStatusCode()
        {
            _favouriteOfferRepositoryMock.Setup(x => x.UserFavouriteOfferExist(It.IsAny<Guid>(), It.IsAny<Guid>()))
                .ReturnsAsync(false);

            var fakeUri = "https://fakeuri.com";

            Environment.SetEnvironmentVariable("OfferExistUri", fakeUri);

            var offerId = Guid.NewGuid();

            _httpMock.When(HttpMethod.Get, $"{fakeUri}{offerId}")
                .Respond(HttpStatusCode.NotFound);

            FavouriteOfferService service = new
                (_userRepositoryMock.Object, _httpMock.ToHttpClient(), _favouriteOfferRepositoryMock.Object);

            await service.Invoking(x => x.CreateFavouriteOffer(Guid.NewGuid(), offerId))
                .Should()
                .ThrowAsync<ResourceNotFoundException>();
        }

        [Fact]
        public async Task Service_CreateFavouriteOffer_Should_CallEntityExistAsyncToRepository_WhenOfferExistReuestIsSuceeded()
        {
            _favouriteOfferRepositoryMock.Setup(x => x.UserFavouriteOfferExist(It.IsAny<Guid>(), It.IsAny<Guid>()))
                .ReturnsAsync(false);

            // To avoid error
            _userRepositoryMock.Setup(x => x.EntityExistAsync(It.IsAny<Guid>()))
                .ReturnsAsync(true);

            var fakeUri = "https://fakeuri.com";

            Environment.SetEnvironmentVariable("OfferExistUri", fakeUri);

            var offerId = Guid.NewGuid();

            _httpMock.When(HttpMethod.Get, $"{fakeUri}{offerId}")
                .Respond(HttpStatusCode.NoContent);

            FavouriteOfferService service = new
                (_userRepositoryMock.Object, _httpMock.ToHttpClient(), _favouriteOfferRepositoryMock.Object);

            await service.CreateFavouriteOffer(Guid.NewGuid(), offerId);

            _userRepositoryMock.Verify(x => x.EntityExistAsync(It.IsAny<Guid>()));
        }

        [Fact]
        public async Task Service_CreateFavouriteOffer_Should_ThrowResourceNotFoundException_WhenUserEntityNotFound()
        {
            _favouriteOfferRepositoryMock.Setup(x => x.UserFavouriteOfferExist(It.IsAny<Guid>(), It.IsAny<Guid>()))
                .ReturnsAsync(false);

            _userRepositoryMock.Setup(x => x.EntityExistAsync(It.IsAny<Guid>()))
                .ReturnsAsync(false); ;

            var fakeUri = "https://fakeuri.com";

            Environment.SetEnvironmentVariable("OfferExistUri", fakeUri);

            var offerId = Guid.NewGuid();

            _httpMock.When(HttpMethod.Get, $"{fakeUri}{offerId}")
                .Respond(HttpStatusCode.NoContent);

            FavouriteOfferService service = new
                (_userRepositoryMock.Object, _httpMock.ToHttpClient(), _favouriteOfferRepositoryMock.Object);


            await service.Invoking(x => x.CreateFavouriteOffer(Guid.NewGuid(), offerId))
                .Should()
                .ThrowAsync<ResourceNotFoundException>();
        }

        [Fact]
        public async Task Service_CreateFavouriteOffer_Should_AddOfferToRepository_WhenNoErrorThrown()
        {
            // Setup to avoid erros

            _favouriteOfferRepositoryMock.Setup(x => x.UserFavouriteOfferExist(It.IsAny<Guid>(), It.IsAny<Guid>()))
                .ReturnsAsync(false);

            _userRepositoryMock.Setup(x => x.EntityExistAsync(It.IsAny<Guid>()))
                .ReturnsAsync(true); ;

            var fakeUri = "https://fakeuri.com";

            Environment.SetEnvironmentVariable("OfferExistUri", fakeUri);

            var offerId = Guid.NewGuid();

            _httpMock.When(HttpMethod.Get, $"{fakeUri}{offerId}")
                .Respond(HttpStatusCode.NoContent);

            FavouriteOfferService service = new
                (_userRepositoryMock.Object, _httpMock.ToHttpClient(), _favouriteOfferRepositoryMock.Object);

            await service.CreateFavouriteOffer(Guid.NewGuid(), offerId);

            _favouriteOfferRepositoryMock.Verify(x => x.Insert(It.IsAny<FavouriteOffer>()), Times.Once);
            _favouriteOfferRepositoryMock.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task Service_DeleteFavouriteOffer_Should_ThrowResourceNotFoundException_WhenUserFavouriteOfferNotFound()
        {
            _favouriteOfferRepositoryMock.Setup(x => x.GetUserFavouriteOffer(It.IsAny<Guid>(), It.IsAny<Guid>()))
                .ReturnsAsync((FavouriteOffer)null!);

            FavouriteOfferService service = new
                (_userRepositoryMock.Object, _httpMock.ToHttpClient(), _favouriteOfferRepositoryMock.Object);

            await service.Invoking(x => x.DeleteFavouriteOffer(Guid.Empty, Guid.Empty))
                .Should()
                .ThrowAsync<ResourceNotFoundException>();
        }

        [Fact]
        public async Task Service_DeleteFavouriteOffer_Should_RemoveFavouriteOffer_WhenUserFavouriteOfferFound()
        {
            _favouriteOfferRepositoryMock.Setup(x => x.GetUserFavouriteOffer(It.IsAny<Guid>(), It.IsAny<Guid>()))
               .ReturnsAsync(new FavouriteOffer());

            FavouriteOfferService service = new
                (_userRepositoryMock.Object, _httpMock.ToHttpClient(), _favouriteOfferRepositoryMock.Object);

            await service.DeleteFavouriteOffer(Guid.Empty, Guid.Empty);

            _favouriteOfferRepositoryMock.Verify(x => x.DeleteEntity(It.IsAny<FavouriteOffer>()) , Times.Once);
            _favouriteOfferRepositoryMock.Verify(x => x.SaveChangesAsync() , Times.Once);
        }
    }
}
