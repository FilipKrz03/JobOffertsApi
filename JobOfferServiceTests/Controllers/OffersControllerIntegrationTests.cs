using JobOffersApiCore.Common;
using JobOffersApiCore.Enum;
using JobOfferServiceTests.Common;
using JobOffersService.DbContexts;
using JobOffersService.Dto;
using JobOffersService.Entities;
using FluentAssertions;
using JobOffersService.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Bogus;

namespace JobOfferServiceTests.Controllers
{
    [Collection("DoNotParallelize")]
    public class OffersControllerIntegrationTests : IntegrationTestBase
    {

        [Fact]
        public async Task Controller_GetJobOfferDetial_ShouldReturnProperJobOfferWith200StatusCode_WhenRequestedOfferExistInDatabase()
        {
            Guid offerGuid = Guid.NewGuid();

            List<Technology> technologies = new()
            {
                new Technology(){Id = Guid.NewGuid() , TechnologyName = "Java"},
                new Technology(){Id = Guid.NewGuid() , TechnologyName = ".NET"}
            };

            JobOffer offer1 = new JobOffer() { Id = offerGuid, Technologies = technologies, OfferCompany = "Cool company" };
            JobOffer offer2 = new JobOffer() { Id = Guid.NewGuid() };

            DbSeeder(db =>
            {
                db.JobOffers.Add(offer1);
                db.SaveChanges();
            });

            var response = await _httpClient.GetAsync($"api/offers/{offerGuid}");

            var result = JsonConvert.DeserializeObject<JobOffer>(await response.Content.ReadAsStringAsync());

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

            result?.Technologies[0].TechnologyName.Should().Be("Java");
            result?.OfferCompany.Should().Be("Cool company");
            result?.Id.Should().Be(offerGuid);
        }

        [Fact]
        public async Task Controller_GetJobOfferDetail_ShouldReturn404Response_WhenRequestedOfferNotExistInDatabase()
        {
            Guid offerGuid = Guid.NewGuid();
            Guid notExistingGuid = Guid.NewGuid();

            List<Technology> technologies = new()
            {
                new Technology(){Id = Guid.NewGuid() , TechnologyName = "Java"},
                new Technology(){Id = Guid.NewGuid() , TechnologyName = ".NET"}
            };

            JobOffer offer1 = new JobOffer() { Id = offerGuid, Technologies = technologies, OfferCompany = "Cool company" };

            DbSeeder(db =>
            {
                db.JobOffers.Add(offer1);
                db.SaveChanges();
            });

            var response = await _httpClient.GetAsync($"api/offers/{notExistingGuid}");

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Controller_GetJobOffers_ShouldReturn200ResponseWithEmptyIEnumerable_WhenDatabaseIsEmpty()
        {
            ClearDatabase();

            var resposne = await _httpClient.GetAsync("api/offers");

            var result = JsonConvert.DeserializeObject<IEnumerable<JobOfferBasicResponse>>
                (await resposne.Content.ReadAsStringAsync());

            resposne.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

            result!.Count().Should().Be(0);
        }

        [Fact]
        public async Task Controller_GetJobOffers_ShouldReturn200ResponseWithListOfJobOffers_WhenDatabaseIsNotEmpty()
        {
            ClearDatabase();

            var offerFake = new Faker<JobOffer>();

            List<JobOffer> offersFake = offerFake.Generate(5);

            DbSeeder(db =>
            {
                db.JobOffers.AddRange(offersFake);
                db.SaveChanges();
            });

            var resposne = await _httpClient.GetAsync("api/offers");

            var result = JsonConvert.DeserializeObject<List<JobOfferBasicResponse>>
                (await resposne.Content.ReadAsStringAsync());

            resposne.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

            result!.Count.Should().Be(5);

            result![0].Id.Should().Be(offersFake[0].Id);
            result![1].Localization.Should().Be(offersFake[1].Localization);
            result![2].OfferCompany.Should().Be(offersFake[2].OfferCompany);
            result![3].OfferLink.Should().Be(offersFake[3].OfferLink);
            result![4].WorkMode.Should().Be(offersFake[4].WorkMode);
        }

        [Fact]
        public async Task
            Controller_GetJobOffers_ShouldReturn200ResponseWithListOfTJobOffersLengthNotGreaterThan50_WhenRequestedPageSizeIsMoreThan50AndDatabaseSizeIsMoreThan50()
        {
            var offerFake = new Faker<JobOffer>();

            List<JobOffer> offersFake = offerFake.Generate(60);

            DbSeeder(db =>
            {
                db.JobOffers.AddRange(offersFake);
                db.SaveChanges();
            });

            var resposne = await _httpClient.GetAsync("api/offers?pageSize=60");

            var result = JsonConvert.DeserializeObject<List<JobOfferBasicResponse>>
                (await resposne.Content.ReadAsStringAsync());

            resposne.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

            result!.Count.Should().Be(50);
        }

        [Fact]
        public async Task
            Controller_GetJobOffers_ShouldReturn200ResponseWithRequestedJobOffersLengthNotGreaterThan50_WhenDatabaseSizeIsSufficient()
        {
            var offerFake = new Faker<JobOffer>();

            List<JobOffer> offersFake = offerFake.Generate(60);

            DbSeeder(db =>
            {
                db.JobOffers.AddRange(offersFake);
                db.SaveChanges();
            });

            var resposne = await _httpClient.GetAsync("api/offers?pageSize=40");

            var result = JsonConvert.DeserializeObject<List<JobOfferBasicResponse>>
                (await resposne.Content.ReadAsStringAsync());

            resposne.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

            result!.Count.Should().Be(40);

        }
    }
}
