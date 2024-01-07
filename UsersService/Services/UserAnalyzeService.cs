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
        private readonly IRabbitMessageProducer _sendEmailToUsersGruopWithRecommendedOffersMessageProducer;

        public UserAnalyzeService(IJobOfferRepository jobOfferRepository, IUserRepository userRepository,
            IRabbitMessageProducer sendEmailToUsersGruopWithRecommendedOffersMessageProducer)
        {
            _jobOfferRepository = jobOfferRepository;
            _userRepository = userRepository;
            _sendEmailToUsersGruopWithRecommendedOffersMessageProducer =
                sendEmailToUsersGruopWithRecommendedOffersMessageProducer;
        }

        public async Task LetUsersKnowAboutNewMatchingOffers()
        {
            DateTime tresholdDate = DateTime.UtcNow - TimeSpan.FromHours(3);

            var newlyCreatedOffers = await _jobOfferRepository.
                GetJobOffersWithTechnologiesFromTresholdDateAsync(tresholdDate);

            var allUsers = await _userRepository.GetAllUsersWithTechnologiesAsync();

            var groupedUsersWithSameTechnologies = allUsers.GroupBy
                (u => string.Join(",", u.Technologies.OrderBy(t => t.TechnologyName).Select(t => t.TechnologyName)));

            foreach (var group in groupedUsersWithSameTechnologies)
            {
                var groupTechnologyNames = group.Key.Split(",");

                var matchingOffers = newlyCreatedOffers
                    .Where(offer => offer.Technologies
                        .Any(tech => groupTechnologyNames.Contains(tech.TechnologyName))).ToList();

                var userEmailsList = group.Select(e => e.Email).ToList();

                var mailContent = CreateEmailContent(matchingOffers);

                var messageObject = new
                {
                    userEmailsList,
                    mailContent
                };

                _sendEmailToUsersGruopWithRecommendedOffersMessageProducer
                    .SendMessage(
                    MAIL_SENDER_EXCHANGE,
                    MAIL_WITH_RECOMENDED_OFFERS_TO_USER_GROUP_ROUTING_KEY,
                    messageObject
                    );
            }
        }

        private string CreateEmailContent(IEnumerable<JobOffer> jobOffers)
        {
            StringBuilder htmlContent = new();

            htmlContent.AppendLine("<!DOCTYPE html>");
            htmlContent.AppendLine("<html lang=\"en\">");
            htmlContent.AppendLine("<head>");
            htmlContent.AppendLine("<meta charset=\"UTF-8\">");
            htmlContent.AppendLine("<meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\">");
            htmlContent.AppendLine("<title>Moja Strona</title>");
            htmlContent.AppendLine("</head>");
            htmlContent.AppendLine("<body>");

            htmlContent.AppendLine
                ("<h1>Hi! Especially for you, we have delivered some offers based on your preferences that might interest you </h1>");

            foreach (var jobOffer in jobOffers)
            {
                htmlContent.AppendLine($"<a href='{jobOffer.OfferLink}'>");
                htmlContent.AppendLine($"<h3>{jobOffer.OfferTitle}</h3>");
                htmlContent.AppendLine($"<p> {jobOffer.OfferCompany}</p>");
                htmlContent.AppendLine("</a>");
            }

            htmlContent.AppendLine("</body>");
            htmlContent.AppendLine("</html>");

            return htmlContent.ToString();
        }
    }
}
