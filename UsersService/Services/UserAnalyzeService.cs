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
            DateTime tresholdDate = DateTime.UtcNow - TimeSpan.FromHours(3);

            var newlyCreatedOffers = 
                await _jobOfferRepository.GetJobOffersFromTresholdDate(tresholdDate);

            Console.WriteLine("Co to bedzie co to bedzie");

            // Map user followed technologies  to new offer technologies

            // Send event to send mail
        }
    }
}
