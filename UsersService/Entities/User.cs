using JobOffersApiCore.BaseObjects;

namespace UsersService.Entities
{
    public class User : BaseEntity 
    {
        public string Email { get; set; } = string.Empty;

        public List<JobOffer> FollowingJobOffers { get; set; } =
            new List<JobOffer>();
    }
}
