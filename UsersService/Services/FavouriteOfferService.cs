
using Microsoft.AspNetCore.Authentication;
using UsersService.Dto;
using UsersService.Interfaces;
using UsersService.Entities;
using AutoMapper;
using JobOffersApiCore.Exceptions;

namespace UsersService.Services
{

    public class FavouriteOfferService : IFavouriteOfferService
    {
        private readonly IUserRepository _userRepository;
        private readonly IFavouriteOfferRepositroy _favouriteOfferRepositroy;
        private readonly HttpClient _httpClient;

        public FavouriteOfferService(IUserRepository userRepository,
            HttpClient httpClient, IFavouriteOfferRepositroy favouriteOfferRepositroy)
        {
            _userRepository = userRepository;
            _httpClient = httpClient;
            _favouriteOfferRepositroy = favouriteOfferRepositroy;
        }

        public async Task CreateFavouriteOffer(Guid userId, Guid offerId)
        {
            bool userOfferExist = await _favouriteOfferRepositroy.UserFavouriteOfferExist(offerId, userId);

            if (userOfferExist)
            {
                throw new ResourceAlreadyExistException
                    ($"Offer with id {offerId} already is saved in you favourite offers !");
            }

            var offerExistUri = Environment.GetEnvironmentVariable("OfferExistUri");

            HttpRequestMessage request =
                new HttpRequestMessage(HttpMethod.Get, $"{offerExistUri}{offerId}");

            request.Headers.Add("ApiKey", Environment.GetEnvironmentVariable("ApiKey"));

            var response = await _httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                throw new ResourceNotFoundException
                    ($"Job offer with id {offerId} do not exist in our database");
            }

            bool userExist = await _userRepository.EntityExistAsync(userId);

            if (userExist == false)
            {
                throw new ResourceNotFoundException("User with id from your token do not exist !");
            }

            FavouriteOffer offer = new()
            {
                OfferId = offerId,
                UserId = userId
            };

            _favouriteOfferRepositroy.Insert(offer);

            await _favouriteOfferRepositroy.SaveChangesAsync();
        }

        public async Task DeleteFavouriteOffer(Guid userId, Guid favouriteOfferId)
        {
            var userFavouriteOffer = await _favouriteOfferRepositroy.GetUserFavouriteOffer(userId, favouriteOfferId);

            if (userFavouriteOffer == null)
            {
                throw new ResourceNotFoundException($"Cannot delete favourite offer because " +
                    $"offer with id {favouriteOfferId} do not exist in your favourite offers");
            }

            _favouriteOfferRepositroy.DeleteEntity(userFavouriteOffer);

            await _favouriteOfferRepositroy.SaveChangesAsync();
        }
    }
}
