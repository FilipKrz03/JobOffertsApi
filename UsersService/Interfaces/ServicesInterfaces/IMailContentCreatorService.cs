using UsersService.Dto;
using UsersService.Entities;

namespace UsersService.Interfaces.ServicesInterfaces
{
    public interface IMailContentCreatorService
    {
        string CreateMailContentForUserWithListOfJobOffersBasedOnPreferations
            (IEnumerable<JobOfferWithLinkCompanyTitleSeniorityTechnologiesDto> jobOffers);

        string CreateMailForUserWithNoSubscribedTechnologies();
    }
}
