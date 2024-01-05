using UsersService.Interfaces.RepositoriesInterfaces;
using UsersService.Interfaces.ServicesInterfaces;

namespace UsersService.Services
{
    public class UserAnalyzeService : IUserAnalyzeService
    {

        private readonly IJobOfferRepository _jobOfferRepository;

        public UserAnalyzeService(IJobOfferRepository jobOfferRepository)
        {
            _jobOfferRepository = jobOfferRepository;
        }

        public async Task LetUsersKnowAboutNewMatchingOffers()
        {
            var newlyCreatedOffers = 
                await _jobOfferRepository.GetNewJobOffers(TimeSpan.FromHours(3));

            // Map user followed technologies  to new offer technologies

            // Send event to send mail
        }
    }
}
