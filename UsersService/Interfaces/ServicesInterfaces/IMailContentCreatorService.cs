using UsersService.Entities;

namespace UsersService.Interfaces.ServicesInterfaces
{
    public interface IMailContentCreatorService
    {
        string CreateMailContentForUserWithListOfJobOffersBasedOnPreferations
            (IEnumerable<JobOffer> jobOffers);

        string CreateMailForUserWithNoSubscribedTechnologies();
    }
}
