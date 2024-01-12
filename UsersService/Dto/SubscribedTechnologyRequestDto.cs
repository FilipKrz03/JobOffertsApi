using UsersService.Dto.ValidationAtrributes;

namespace UsersService.Dto
{
    public class SubscribedTechnologyRequestDto
    {
        [RequireNonDefault(ErrorMessage = "You did not provide valid TechnologyId")]
        public Guid TechnologyId { get; set; }
    }
}
