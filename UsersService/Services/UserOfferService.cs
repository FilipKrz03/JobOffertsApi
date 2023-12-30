
using Microsoft.AspNetCore.Authentication;
using UsersService.Dto;
using UsersService.Interfaces;
using UsersService.Entities;
using AutoMapper;
using JobOffersApiCore.Exceptions;

namespace UsersService.Services
{

    public class UserOfferService : IUserOffersService
    {
        private readonly IUserRepository _userRepository;
        private readonly IFavouriteOfferRepositroy _favouriteOfferRepositroy;
        private readonly IMapper _mapper;
        private readonly HttpClient _httpClient;

        public UserOfferService(IUserRepository userRepository , IMapper mapper , 
            HttpClient httpClient , IFavouriteOfferRepositroy favouriteOfferRepositroy)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _httpClient = httpClient;   
            _favouriteOfferRepositroy = favouriteOfferRepositroy;
        }

        public async Task CreateUserFavouriteOffer(Guid userId, Guid offerId)
        {
            bool userOfferExist = await _userRepository.UserFavouriteOfferExist(offerId, userId);

            if (userOfferExist)
            {
                throw new ResourceAlreadyExistException
                    ($"Offer with id {offerId} already is saved in you favourite offers !");
            }

            var offerExistUri = Environment.GetEnvironmentVariable("OfferExistUri");

            var request = await _httpClient.GetAsync($"{offerExistUri}{offerId}");

            if(!request.IsSuccessStatusCode)
            {
                throw new ResourceNotFoundException
                    ($"Job offer with id {offerId} do not exist in our database");
            }

            var user = await _userRepository.GetById(userId);

            if(user == null)
            {
                throw new ResourceNotFoundException("User with id from your token do not exist !");
            }

            FavouriteOffer offer = new()
            {
                OfferId = offerId,
                UserId = user.Id,
            };

            _favouriteOfferRepositroy.Insert(offer);

            await _favouriteOfferRepositroy.SaveChangesAsync();   
        }

        public async Task DeleteUserFavouriteOffer(Guid offerId , string userIdentity)
        {
         
        }
    }
}
