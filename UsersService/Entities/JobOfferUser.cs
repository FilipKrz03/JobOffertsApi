namespace UsersService.Entities
{
    public class JobOfferUser
    {
        public Guid JobOfferId { get; set; }
        public Guid UserId { get; set; }

        public JobOffer JobOffer { get; set; } = null!;
        public User User { get; set; } = null!;
    }
}
