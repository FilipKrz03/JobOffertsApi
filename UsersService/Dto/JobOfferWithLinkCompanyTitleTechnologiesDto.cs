using JobOffersApiCore.Enum;
using UsersService.Entities;

namespace UsersService.Dto
{
    public record JobOfferWithLinkCompanyTitleSeniorityTechnologiesDto
        (string OfferLink, string OfferTitle, string OfferCompany, Seniority Seniority, List<Technology> Technologies)
    { }
}
