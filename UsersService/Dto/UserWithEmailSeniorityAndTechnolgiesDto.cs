using JobOffersApiCore.Enum;
using UsersService.Entities;

namespace UsersService.Dto
{
    public record UserWithEmailSeniorityAndTechnolgiesDto
        (string Email, Seniority DesiredSeniority, List<Technology> Technologies)
    { }
   
}
