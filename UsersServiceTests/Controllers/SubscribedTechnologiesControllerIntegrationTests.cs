using FluentAssertions;
using Newtonsoft.Json;
using System.Text.Json.Serialization;
using UsersService.Dto;
using UsersService.Entities;
using UsersServiceTests.Common;

namespace UsersServiceTests.Controllers
{
    public class SubscribedTechnologiesControllerIntegrationTests : IntegrationTestBase
    {
        [Fact]
        public async Task Controller_AddSubscribedTechnology_Should_Return400StatusCode_WhenBadRequestBody()
        {
            SetJwtUserEntitieId(Guid.NewGuid());

            var badRequest = new
            {
                badField = "badField"
            };

            var request = await _httpClient.PostAsJsonAsync("api/subscribedtechnologies", badRequest);

            request.StatusCode
                .Should()
                .Be(System.Net.HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task
            Controller_AddSubscribedTechnologies_Should_AddSubscribedTechnologyToUser_WhenTechnologyExistAndUserDoNotHaveThisTechnologyAlredySaved()
        {
            var userId = Guid.NewGuid();

            SetJwtUserEntitieId(userId);

            var technologyId = Guid.NewGuid();

            User userEntitie = new() { Id = userId };

            DbSeeder(db =>
            {
                db.Users.Add(userEntitie);
                db.Technologies.Add(new Technology { Id = technologyId });

                db.SaveChanges();
            });

            SubscribedTechnologyRequestDto body = new() { TechnologyId = technologyId };

            var request = await _httpClient.PostAsJsonAsync("api/subscribedtechnologies", body);

            var db = DbContextGetter();

            var userSubscribedTechnologie =
                db.TechnologyUsers
                .Where(x => x.TechnologyId == technologyId && x.UserId == userId)
                .FirstOrDefault();

            userSubscribedTechnologie
                .Should()
                .NotBeNull();

            userSubscribedTechnologie!.TechnologyId
                .Should()
                .Be(technologyId);

            userSubscribedTechnologie!.UserId
                .Should()
                .Be(userId);
        }

        [Fact]
        public async Task Controller_AddSubscribedTechnology_Should_Return409Response_WhenUserAlreadySubscribingTechnology()
        {
            var userId = Guid.NewGuid();

            SetJwtUserEntitieId(userId);

            var technologyId = Guid.NewGuid();

            User userEntitie = new() { Id = userId };

            DbSeeder(db =>
            {
                db.Users.Add(userEntitie);
                db.Technologies.Add(new Technology
                {
                    Id = technologyId,
                    Users = new List<User> { userEntitie }
                });

                db.SaveChanges();
            });

            SubscribedTechnologyRequestDto body = new() { TechnologyId = technologyId };

            var request = await _httpClient.PostAsJsonAsync("api/subscribedtechnologies", body);

            request.StatusCode
                .Should()
                .Be(System.Net.HttpStatusCode.Conflict);
        }

        [Fact]
        public async Task Controller_AddSubscribedTechnology_Should_Return404Response_WhenTechnologyNotExist()
        {
            var userId = Guid.NewGuid();

            SetJwtUserEntitieId(userId);

            var technologyId = Guid.NewGuid();

            DbSeeder(db =>
            {
                db.Users.Add(new User() { Id = userId});

                db.SaveChanges();
            }); 

            SubscribedTechnologyRequestDto body = new() { TechnologyId = Guid.NewGuid() };

            var request = await _httpClient.PostAsJsonAsync("api/subscribedtechnologies", body);

            request.StatusCode
                .Should()
                .Be(System.Net.HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task 
            Controller_DeleteSubscribedTechnology_Should_Returun404StatusCode_WhenSubscribedTechnologyAlreadyDoNotExist()
        {
            var userId = Guid.NewGuid();

            SetJwtUserEntitieId(userId);

            DbSeeder(db =>
            {
                db.Users.Add(new User() { Id = userId });

                db.SaveChanges();
            });

            var request = await _httpClient.DeleteAsync($"api/subscribedtechnologies/{Guid.NewGuid()}");

            request.StatusCode
                .Should()
                .Be(System.Net.HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task
            Controller_DeleteSubscribedTechnology_Should_ProperlyDeleteSubscribedTechnologyAndReturn204StatusCode_WhenUserSubscribedTechnologyExist()
        {
            var userId = Guid.NewGuid();

            SetJwtUserEntitieId(userId);

            var technologyId = Guid.NewGuid();

            User userEntitie = new() { Id = userId };

            DbSeeder(db =>
            {
                db.Users.Add(userEntitie);
                db.Technologies.Add(new Technology
                {
                    Id = technologyId,
                    Users = new List<User> { userEntitie }
                });

                db.SaveChanges();
            });

            var request = await _httpClient.DeleteAsync($"api/subscribedtechnologies/{technologyId}");

            var db = DbContextGetter();

            var userTechnology = db.TechnologyUsers
                .Where(x => x.UserId == userId && x.TechnologyId == technologyId)
                .FirstOrDefault();

            userTechnology
                .Should()
                .BeNull();

            request.StatusCode
                .Should()
                .Be(System.Net.HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task Controller_GetSubscribedTechnologies_Should_Return200Response()
        {
            SetJwtUserEntitieId(Guid.NewGuid());

            var reqeust = await _httpClient.GetAsync("api/subscribedtechnologies");

            reqeust.StatusCode
                .Should()
                .Be(System.Net.HttpStatusCode.OK);
        }

        [Fact]
        public async Task Controller_GetSubscribedTechnologies_Should_ReturnAllProperlySortedUserTechnologies()
        {
            var userId = Guid.NewGuid();

            SetJwtUserEntitieId(userId);

            User userEntitiy = new() { Id = userId };

            string techName1 = "atech";
            string techName2 = "btech";
            string techName3 = "ctech";

            DbSeeder(db =>
            {
                db.Users.Add(userEntitiy);

                db.Technologies.Add(new Technology()
                {
                    Id = Guid.NewGuid(),
                    TechnologyName = techName2,
                    Users = new List<User> { userEntitiy }
                });

                db.Technologies.Add(new Technology()
                {
                    Id = Guid.NewGuid(),
                    TechnologyName = techName3,
                    Users = new List<User> { userEntitiy }
                });

                db.Technologies.Add(new Technology()
                {
                    Id = Guid.NewGuid(),
                    TechnologyName = techName1,
                    Users = new List<User> { userEntitiy }
                });

                db.SaveChanges();
            });

            var request = await _httpClient.GetAsync("api/subscribedtechnologies?sortColumn=name&sortOrder=desc");

            var data = JsonConvert.DeserializeObject<List<TechnologyBasicResponseDto>>
                (await request.Content.ReadAsStringAsync());

            data![0].TechnologyName
                .Should()
                .Be(techName3);

            data![1].TechnologyName
                .Should()
                .Be(techName2);

            data![2].TechnologyName
                .Should()
                .Be(techName1);
        }
    }
}
