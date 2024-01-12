using UsersService.Dto;
using UsersService.Entities;
using UsersServiceTests.Common;
using JobOffersApiCore.Enum;
using FluentAssertions;
using Microsoft.AspNetCore.Routing.Matching;

namespace UsersServiceTests.Controllers
{
    public class UserControllerIntegrationTests : IntegrationTestBase
    {

        [Fact]
        public async Task Controller_PutSeniority_Should_ChangeUserSeniorityAndReturn204Response()
        {
            Guid userId = Guid.NewGuid();

            SetJwtUserEntitieId(userId);

            DbSeeder(db =>
            {
                db.Users.Add(new User()
                {
                    Id = userId,
                    DesiredSeniority = Seniority.Junior
                });

                db.SaveChanges();
            });

            PutSeniorityRequestDto body = new(Seniority.Senior);

            var request = await _httpClient.PutAsJsonAsync("api/user/seniority", body);

            var db = DbContextGetter();

            var userWithChangedSeniority =
                db.Users
                .Where(x => x.Id == userId)
                .FirstOrDefault();

            userWithChangedSeniority!.DesiredSeniority
                .Should()
                .Be(Seniority.Senior);

            request.StatusCode
                .Should()
                .Be(System.Net.HttpStatusCode.NoContent);
        }
    }
}
