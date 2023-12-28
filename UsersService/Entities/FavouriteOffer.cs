using JobOffersApiCore.BaseObjects;

namespace UsersService.Entities
{
    public class FavouriteOffer : BaseEntity
    {
        public Guid OfferId {  get; set; }  

        public Guid UserId { get; set; }
        public User User { get; set; } = null!;
    }
}
