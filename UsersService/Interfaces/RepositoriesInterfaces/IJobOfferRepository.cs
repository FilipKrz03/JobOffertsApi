using JobOffersApiCore.Common;
using JobOffersApiCore.Interfaces;
using System.Linq.Expressions;
using UsersService.Dto;
using UsersService.Entities;

namespace UsersService.Interfaces.RepositoriesInterfaces
{
    public interface IJobOfferRepository : IBaseRepository<JobOffer>
    {
        Task<JobOffer?> GetUserJobOfferAsync(Guid userId, Guid jobOfferId);
        Task<PagedList<JobOffer>> GetUserJobOffersAsync
           (Expression<Func<JobOffer, object>> keySelector, ResourceParamethers resourceParamethers, Guid userId);

        Task<IEnumerable<JobOfferWithLinkCompanyTitleSeniorityTechnologiesDto>> 
            GetJobOffersWithLinkCompanyTitleSeniorityTechnologiesFromTresholdDateAsync(DateTime tresholdDate);
    }
}
