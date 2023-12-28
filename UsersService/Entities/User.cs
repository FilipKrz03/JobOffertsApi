using JobOffersApiCore.BaseObjects;

namespace UsersService.Entities
{
    public class User : BaseEntity 
    {
        public string Email {  get; set; }  = string.Empty;

        public string IdentityId { get; set; } = string.Empty;

        public ICollection<FavouriteOffer> FavouriteOffers { get; set; }  = 
            new List<FavouriteOffer>();
    }
}
