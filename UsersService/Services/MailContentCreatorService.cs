using System.Text;
using UsersService.Entities;
using UsersService.Interfaces.ServicesInterfaces;

namespace UsersService.Services
{
    public class MailContentCreatorService : IMailContentCreatorService
    {
        public string CreateMailContentForUserWithListOfJobOffersBasedOnPreferations
            (IEnumerable<JobOffer> jobOffers)
        {
            StringBuilder htmlContent = new();

            htmlContent.AppendLine("<!DOCTYPE html>");
            htmlContent.AppendLine("<html lang=\"en\">");
            htmlContent.AppendLine("<head>");
            htmlContent.AppendLine("<meta charset=\"UTF-8\">");
            htmlContent.AppendLine("<meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\">");
            htmlContent.AppendLine("<title>Job offers</title>");
            htmlContent.AppendLine("</head>");
            htmlContent.AppendLine("<body>");

            htmlContent.AppendLine
                ("<h1>Hi! Especially for you, we have delivered some offers based on your preferences that might interest you </h1>");

            if (!jobOffers.Any())
            {
                htmlContent.AppendLine("<h2> Currently we do not have anny offers for you ! " +
                    "But do not worry . We will find something for you ASAP. Stay tuned ! </h2>");
            }

            foreach (var jobOffer in jobOffers)
            {
                htmlContent.AppendLine($"<a href='{jobOffer.OfferLink}'>");
                htmlContent.AppendLine
                    ($"<div> <h3 style='display: inline;'> {jobOffer.OfferTitle} - </h3>");
                htmlContent.
                    AppendLine($"<p style='display: inline;'>{jobOffer.OfferCompany}</p> </div>");
                htmlContent.AppendLine("</a>");
            }

            htmlContent.AppendLine("</body>");
            htmlContent.AppendLine("</html>");

            return htmlContent.ToString();
        }

        public string CreateMailForUserWithNoSubscribedTechnologies()
        {
            StringBuilder htmlContent = new();

            htmlContent.AppendLine("<!DOCTYPE html>");
            htmlContent.AppendLine("<html lang=\"en\">");
            htmlContent.AppendLine("<head>");
            htmlContent.AppendLine("<meta charset=\"UTF-8\">");
            htmlContent.AppendLine("<meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\">");
            htmlContent.AppendLine("<title>Job offers</title>");
            htmlContent.AppendLine("</head>");
            htmlContent.AppendLine("<body>");

            htmlContent.AppendLine("<h1>Your preferations.</h1>");
            htmlContent.AppendLine
                ("<p> Hey tell us about your technolgies. Based on this we will deliver to you Job offers that matching your skills !");

            htmlContent.AppendLine("</body>");
            htmlContent.AppendLine("</html>");

            return htmlContent.ToString();
        }
    }
}
