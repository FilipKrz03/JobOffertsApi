using JobOffersApiCore.Interfaces;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using UsersService.Entities;
using UsersService.Interfaces.RepositoriesInterfaces;
using UsersService.Interfaces.ServicesInterfaces;
using static UsersService.Props.RabbitMqMailSendProps;

namespace UsersService.Services
{
    public class UserAnalyzeService : IUserAnalyzeService
    {

        private readonly IJobOfferRepository _jobOfferRepository;
        private readonly IUserRepository _userRepository;
        private readonly IRabbitMessageProducer _sendEmailMessageProducer;
        private readonly IMailContentCreatorService _mailContentCreatorService;

        public UserAnalyzeService(
            IJobOfferRepository jobOfferRepository, 
            IUserRepository userRepository,
            IRabbitMessageProducer sendEmailMessageProducer , 
            IMailContentCreatorService mailContentCreatorService
            )
        {
            _jobOfferRepository = jobOfferRepository;
            _userRepository = userRepository;
            _sendEmailMessageProducer = sendEmailMessageProducer;
            _mailContentCreatorService = mailContentCreatorService;
        }

        public async Task LetUsersKnowAboutNewMatchingOffers()
        {
            DateTime tresholdDate = DateTime.UtcNow - TimeSpan.FromHours(150);

            var newlyCreatedOffers = await _jobOfferRepository.
                GetJobOffersWithTechnologiesFromTresholdDateAsync(tresholdDate);

            var allUsers = await _userRepository.GetAllUsersWithTechnologiesAsync();

            var usersWithSubscribedTechnologies = allUsers.Where(user => user.Technologies.Count > 0);
            var usersWithoutSubscribedTechnologies = allUsers.Where(user => user.Technologies.Count == 0);

            if(usersWithoutSubscribedTechnologies.Any())
            {
                var mailObject = new
                {
                    emailsList = usersWithoutSubscribedTechnologies.Select(e => e.Email).ToList() , 
                    mailContent = _mailContentCreatorService.CreateMailForUserWithNoSubscribedTechnologies(), 
                    subject = "Tell us about yourself !"
                };

                _sendEmailMessageProducer.SendMessage(
                MAIL_SEND_EXCHANGE,
                MAIL_SEND_ROUTING_KEY,
                mailObject
                );
            }

            var groupedUsersWithSameTechnologies = usersWithSubscribedTechnologies.GroupBy
                (u => string.Join(",", u.Technologies.OrderBy(t => t.TechnologyName).Select(t => t.TechnologyName)));

            foreach (var group in groupedUsersWithSameTechnologies)
            {
                var groupTechnologyNames = group.Key.Split(",");

                var matchingOffers = newlyCreatedOffers
                    .Where(offer => offer.Technologies
                        .Any(tech => groupTechnologyNames.Contains(tech.TechnologyName))).ToList();
         
                var mailContent = _mailContentCreatorService
                    .CreateMailContentForUserWithListOfJobOffersBasedOnPreferations(matchingOffers);

                var mailObject = new
                {
                    emailsList = group.Select(e => e.Email).ToList() ,
                    mailContent ,
                    subject = "Offers for you !"
                };

                _sendEmailMessageProducer.SendMessage(
                    MAIL_SEND_EXCHANGE,
                    MAIL_SEND_ROUTING_KEY,
                    mailObject
                    );
            }
        }
    }
}
