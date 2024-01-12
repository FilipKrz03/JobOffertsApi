using UsersService.Dto.ValidationAtrributes;

namespace UsersService.Dto
{
    public class FollowedOfferToAddRequestDto
    {
        [RequireNonDefault(ErrorMessage = "You did not provide valid Offer Id")]
        public Guid OfferId { get; set; }

        public FollowedOfferToAddRequestDto(Guid offerId)
        {
            OfferId = offerId;
        }
    }
}