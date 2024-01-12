using FluentAssertions;
using UsersServiceTests.Common;
using UsersService.Entities;
using Newtonsoft.Json;
using JobOffersApiCore.Common;
using UsersService.Dto;
using Microsoft.EntityFrameworkCore;
using System.Drawing.Drawing2D;

namespace UsersServiceTests.Controllers
{
    public class FollowedJobOfferControllerIntegrationTests : IntegrationTestBase
    {
        [Fact]
        public async Task Controller_GetFollowedJobOffers_Should_Return200StatusCode()
        {
            Guid userId = Guid.NewGuid();

            SetJwtUserEntitieId(userId);

            var request = await _httpClient.GetAsync("api/followedjoboffers");

            request.StatusCode
                .Should()
                .Be(System.Net.HttpStatusCode.OK);
        }

        [Fact]
        public async Task Controller_GetFollowedJobOffers_Should_ReturnAllUsersFollowedOffers()
        {
            Guid userId = Guid.NewGuid();

            SetJwtUserEntitieId(userId);

            User userEntitie = new() { Id = userId };

            List<User> userListWithUserEntitie = new() { userEntitie };

            string localization = "Krakow";
            string company = "coolCompany";

            DbSeeder(db =>
            {
                db.Users.Add(userEntitie);

                // Should be in result
                db.JobOffers.Add(new JobOffer()
                {
                    Id = Guid.NewGuid(),
                    Users = userListWithUserEntitie,
                    Localization = localization
                });
                db.JobOffers.Add(new JobOffer()
                {
                    Id = Guid.NewGuid(),
                    Users = userListWithUserEntitie,
                    OfferCompany = company
                });

                //Should not be in result
                db.JobOffers.Add(new JobOffer() { Id = Guid.NewGuid() });
                db.JobOffers.Add(new JobOffer() { Id = Guid.NewGuid() });

                db.SaveChanges();
            });

            var request = await _httpClient.GetAsync("api/followedjoboffers");

            var data = JsonConvert.DeserializeObject<List<JobOfferBasicResponseDto>>
                (await request.Content.ReadAsStringAsync());

            data!.Count
                .Should()
                .Be(2);

            data![0].Localization
                .Should()
                .Be(localization);

            data![1].OfferCompany
                .Should()
                .Be(company);
        }

        [Fact]
        public async Task Controller_GetFollowedJobOffers_ShouldReturnProperlySortedAllUsersFollowedOffers()
        {
            Guid userId = Guid.NewGuid();

            SetJwtUserEntitieId(userId);

            User userEntitie = new() { Id = userId };

            List<User> userListWithUserEntitie = new() { userEntitie };

            DbSeeder(db =>
            {
                db.Users.Add(userEntitie);

                // Should be in result

                db.JobOffers.Add(new JobOffer()
                {
                    Id = Guid.NewGuid(),
                    Users = userListWithUserEntitie,
                    EarningsFrom = 4000,
                    EarningsTo = 5000,
                });

                db.JobOffers.Add(new JobOffer()
                {
                    Id = Guid.NewGuid(),
                    Users = userListWithUserEntitie,
                    EarningsFrom = 10000,
                    EarningsTo = 11000
                });

                db.JobOffers.Add(new JobOffer()
                {
                    Id = Guid.NewGuid(),
                    Users = userListWithUserEntitie,
                    EarningsFrom = 3000,
                    EarningsTo = 4000,
                });

                db.SaveChanges();
            });

            var request = await _httpClient.GetAsync("api/followedjoboffers?sortColumn=earnings&sortOrder=asc");

            var data = JsonConvert.DeserializeObject<List<JobOfferBasicResponseDto>>
                (await request.Content.ReadAsStringAsync());

            data![0].PaymentRange
                .Should()
                .Contain("3000-4000");

            data![1].PaymentRange
                .Should()
                .Contain("4000-5000");

            data![2].PaymentRange
                .Should()
                .Contain("10000-11000");
        }

        [Fact]
        public async Task Controller_GetFollowedJobOffer_Should_Retur404Response_WhenUserFollowedJobOfferNotFound()
        {
            SetJwtUserEntitieId(Guid.NewGuid());

            // Not adding any user favourite job offers

            var request = await _httpClient.GetAsync($"api/followedjoboffers/{Guid.NewGuid()}");

            request.StatusCode
                .Should()
                .Be(System.Net.HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Controller_GetFollowedJobOffer_Should_Return200ResposneWithJobOffer_WhenFollowedJobOfferFound()
        {
            Guid userId = Guid.NewGuid();

            SetJwtUserEntitieId(userId);

            Guid offerId = Guid.NewGuid();

            User userEntite = new() { Id = userId };

            string workMode = "hybrid";

            DbSeeder(db =>
            {
                db.Users.Add(userEntite);
                db.JobOffers.Add(new JobOffer()
                {
                    Id = offerId,
                    Users = new List<User>() { userEntite },
                    WorkMode = workMode
                });

                db.SaveChanges();
            });

            var request = await _httpClient.GetAsync($"api/followedjoboffers/{offerId}");

            var data = JsonConvert.DeserializeObject<JobOfferDetailResponseDto>
                (await request.Content.ReadAsStringAsync());

            data!.Id
                .Should()
                .Be(offerId);

            data.WorkMode
                .Should()
                .Be(workMode);
        }


        [Fact]
        public async Task Controller_DeleteFollowedJobOffer_Should_Return404Response_WhenUserFavouriteOfferToDeleteDoNotExist()
        {
            SetJwtUserEntitieId(Guid.NewGuid());

            var request = await _httpClient.DeleteAsync($"api/followedjoboffers/{Guid.NewGuid()}");

            request.StatusCode
                .Should()
                .Be(System.Net.HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Controller_DeleteFollowedJobOffer_Should_DeleteFollowedOfferFromDbAndReturn204_WhenUserFavouriteOfferToDeleteExist()
        {
            Guid userId = Guid.NewGuid();

            SetJwtUserEntitieId(userId);

            Guid offerId = Guid.NewGuid();

            User userEntite = new() { Id = userId };

            string workMode = "hybrid";

            DbSeeder(db =>
            {
                db.Users.Add(userEntite);
                db.JobOffers.Add(new JobOffer()
                {
                    Id = offerId,
                    Users = new List<User>() { userEntite },
                    WorkMode = workMode
                });


                db.SaveChanges();
            });

            var request = await _httpClient.DeleteAsync($"api/followedjoboffers/{offerId}");

            var db = DbContextGetter();

            var offerAferDeletiaon = db.JobOfferUsers
                .Where(j => j.JobOfferId == offerId && j.UserId == userId)
                .FirstOrDefault();

            offerAferDeletiaon.Should()
                .BeNull();

            request.StatusCode
                .Should()
                .Be(System.Net.HttpStatusCode.NoContent);
        }


        [Fact]
        public async Task
            Controller_AddFollowedJobOffer_Should_ProperlyAddFollowedJobOfferToDbAndReturn201StatusCode_When_FollowedOfferToAddExistAndIsNotAlreadySavedInUserFollowedOffers()
        {
            Guid userId = Guid.NewGuid();

            SetJwtUserEntitieId(userId);

            Guid offerId = Guid.NewGuid();

            User userEntite = new() { Id = userId };

            string workMode = "hybrid";

            DbSeeder(db =>
            {
                db.Users.Add(userEntite);

                // To add followedJobOffer to user first this job offer need to be avaliable in db
                db.JobOffers.Add(new JobOffer()
                {
                    Id = offerId,
                    WorkMode = workMode
                });

                db.SaveChanges();
            });

            FollowedOfferToAddRequestDto body = new(offerId);

            var request = await _httpClient.PostAsJsonAsync("api/followedjoboffers", body);

            var db = DbContextGetter();

            var addedJobOffer = db.JobOfferUsers
                .Where(x => x.UserId == userId && x.JobOfferId == offerId)
                .FirstOrDefault();

            addedJobOffer
                .Should()
                .NotBeNull();

            request.StatusCode
                .Should()
                .Be(System.Net.HttpStatusCode.Created);
        }

        [Fact]
        public async Task Controller_AddFollowedJobOffer_Should_Return404StatusCode_When_FollowedOfferToAddDoNotExistInDatabase()
        {
            Guid userId = Guid.NewGuid();

            SetJwtUserEntitieId(userId);

            DbSeeder(db =>
            {
                db.Users.Add(new User() { Id = userId});

                // No user job offers

                db.SaveChanges();
            });

            FollowedOfferToAddRequestDto body = new(Guid.NewGuid());

            var request = await _httpClient.PostAsJsonAsync("api/followedjoboffers", body);

            request.StatusCode
                .Should()
                .Be(System.Net.HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task
            Controller_AddFollowedJobOffer_Should_Return409StatusCode_When_FollowedJobOfferToAddExistInDatabaseAndIsAlreadySavedInUserFollowedOffers()
        {
            Guid userId = Guid.NewGuid();

            SetJwtUserEntitieId(userId);

            Guid offerId = Guid.NewGuid();

            User userEntite = new() { Id = userId };

            string workMode = "hybrid";

            DbSeeder(db =>
            {
                db.Users.Add(userEntite);

                db.JobOffers.Add(new JobOffer()
                {
                    Id = offerId,
                    WorkMode = workMode,
                    Users = new List<User>() { userEntite }
                });

                db.SaveChanges();
            });

            FollowedOfferToAddRequestDto body = new(offerId);

            var request = await _httpClient.PostAsJsonAsync("api/followedjoboffers", body);

            request.StatusCode
                .Should()
                .Be(System.Net.HttpStatusCode.Conflict);
        }

        [Fact]
        public async Task Controller_AddFollowedJobOffer_ShouldReturn400Response_WhenBadRequestBody()
        {
            SetJwtUserEntitieId(Guid.NewGuid());

            var badBody = new
            {
                badProperty = "bad prop"
            };

            var request = await _httpClient.PostAsJsonAsync("api/followedjoboffers", badBody);

            request.StatusCode
                .Should()
                .Be(System.Net.HttpStatusCode.BadRequest);
        }
    }
}
