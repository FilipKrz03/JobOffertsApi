using JobOffersApiCore.Enum;
using System.ComponentModel.DataAnnotations;

namespace UsersService.Dto
{
    public record PutSeniorityRequestDto(Seniority Seniority)
    {
    }
}
