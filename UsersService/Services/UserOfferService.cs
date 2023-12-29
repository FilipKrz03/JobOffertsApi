
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
        private readonly IMapper _mapper;
        private readonly HttpClient _httpClient;

        public UserOfferService(IUserRepository userRepository , IMapper mapper , 
            HttpClient httpClient)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _httpClient = httpClient;   
        }

        public async Task CreateUserFavouriteOffer(string userId, Guid offerId)
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

        
            // Fetch user entitie
            // Add job offer to user entite

            //Save changes on db context
        }
    }
}
