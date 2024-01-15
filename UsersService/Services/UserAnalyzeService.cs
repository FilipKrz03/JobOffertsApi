using JobOffersApiCore.Enum;
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
            IRabbitMessageProducer sendEmailMessageProducer,
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
            DateTime tresholdDate = DateTime.UtcNow - TimeSpan.FromHours(3);

            var newlyCreatedOffers = await _jobOfferRepository.
              GetJobOffersWithLinkCompanyTitleSeniorityTechnologiesFromTresholdDateAsync(tresholdDate);

            var allUsers = await
                _userRepository.GetAllUsersWithEmailSeniorityAndTechnologiesAsync();

            if (!allUsers.Any()) return;

            var usersWithSubscribedTechnologies = allUsers.Where(user => user.Technologies.Count > 0);
            var usersWithoutSubscribedTechnologies = allUsers.Where(user => user.Technologies.Count == 0);

            if (usersWithoutSubscribedTechnologies.Any())
            {
                var mailObject = new
                {
                    emailsList = usersWithoutSubscribedTechnologies.Select(e => e.Email).ToList(),
                    mailContent = _mailContentCreatorService.CreateMailForUserWithNoSubscribedTechnologies(),
                    subject = "Tell us about yourself !"
                };

                _sendEmailMessageProducer.SendMessage(
                MAIL_SEND_EXCHANGE,
                MAIL_SEND_ROUTING_KEY,
                mailObject
                );
            }

            var groupedUsersWithSameTechnologiesAndSeniority = usersWithSubscribedTechnologies.GroupBy
                (u => new
                {
                    Technologies = string.Join(",", u.Technologies.OrderBy(t => t.TechnologyName).Select(t => t.TechnologyName)),
                    Seniority = u.DesiredSeniority
                });

            foreach (var group in groupedUsersWithSameTechnologiesAndSeniority)
            {
                var groupTechnologyNames = group.Key.Technologies.Split(",");

                var matchingOffers = newlyCreatedOffers
                    .Where(offer => offer.Technologies
                        .Any(tech => groupTechnologyNames.Contains(tech.TechnologyName))).ToList();

                //If user desired seniority is unkown than we do not want to limit job offers based on seniority level
                if (group.Key.Seniority != Seniority.Unknown)
                {
                    matchingOffers = matchingOffers.Where(
                        offer => offer.Seniority == group.Key.Seniority || offer.Seniority == Seniority.Unknown
                        ).ToList();
                    // If offer seniority is Unkown then we want to include it 
                }

                var mailContent = _mailContentCreatorService
                    .CreateMailContentForUserWithListOfJobOffersBasedOnPreferations(matchingOffers);

                var mailObject = new
                {
                    emailsList = group.Select(e => e.Email).ToList(),
                    mailContent,
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
