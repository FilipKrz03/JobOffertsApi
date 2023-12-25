using Bogus;
using FluentAssertions;
using JobOfferServiceTests.Common;
using JobOffersService.Dto;
using JobOffersService.Entities;
using Newtonsoft.Json;

namespace JobOfferServiceTests.Controllers
{
    [Collection("DoNotParallelize")]
    public class TechnologyControllerIntegrationTests : IntegrationTestBase
    {
        [Fact]
        public async Task Controller_GetTechnologyWitJobOffers_Should_ReturnProperTechnolgyWith200StatusCode_WhenRequestedTechnologyExistInDatabase()
        {
            Guid techGuid = Guid.NewGuid();

            List<Technology> technologies = new()
            {
                new Technology(){Id = techGuid , TechnologyName = "Java"},
                new Technology(){Id = Guid.NewGuid() , TechnologyName = ".NET"}
            };

            List<JobOffer> jobOffers = new()
            {
               new JobOffer() { Id = Guid.NewGuid(), Technologies = technologies, OfferCompany = "Cool company" } ,
               new JobOffer() { Id = Guid.NewGuid() }
            };

            technologies[0].JobOffers = jobOffers;

            DbSeeder(db =>
            {
                db.Technologies.AddRange(technologies);
                db.SaveChanges();
            });

            var response = await _httpClient.GetAsync($"api/technologies/{techGuid}");

            var result = JsonConvert.DeserializeObject<Technology>(await response.Content.ReadAsStringAsync());

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

            result?.TechnologyName.Should().Be("Java");
            result?.Id.Should().Be(techGuid);
            result?.JobOffers[0].OfferCompany.Should().Be("Cool company");
        }

        [Fact]
        public async Task Controller_GetTechnologyWithJobOffersList_ShouldReturn404Response_WhenRequestedTechnologyNotExistInDatabase()
        {
            Guid techGuid = Guid.NewGuid();
            Guid notExistingGuid = Guid.NewGuid();

            var technology = new Technology() { Id = techGuid, TechnologyName = "Java" };
       
            DbSeeder(db =>
            {
                db.Technologies.Add(technology);
                db.SaveChanges();
            });

            var response = await _httpClient.GetAsync($"api/technologies/{notExistingGuid}");

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Controller_GetTechnologies_ShouldReturn200ResponseWithEmptyIEnumerable_WhenDatabaseIsEmpty()
        {
            ClearDatabase();

            var resposne = await _httpClient.GetAsync("api/technologies");

            var result = JsonConvert.DeserializeObject<IEnumerable<TechnologyBasicResponse>>
                (await resposne.Content.ReadAsStringAsync());

            resposne.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

            result!.Count().Should().Be(0);
        }

        [Fact]
        public async Task Controller_GetTechnologies_ShouldReturn200ResponseWithListOfTechnologies_WhenDatabaseIsNotEmpty()
        {
            ClearDatabase();

            var techFake = new Faker<Technology>();

            List<Technology> techList = techFake.Generate(5);

            DbSeeder(db =>
            {
                db.Technologies.AddRange(techList);
                db.SaveChanges();
            });

            var resposne = await _httpClient.GetAsync("api/technologies");

            var result = JsonConvert.DeserializeObject<List<TechnologyBasicResponse>>
                (await resposne.Content.ReadAsStringAsync());

            resposne.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

            result!.Count.Should().Be(5);

            result![0].Id.Should().Be(techList[0].Id);
            result![1].TechnologyName.Should().Be(techList[1].TechnologyName);
            result![2].Id.Should().Be(techList[2].Id);
            result![3].TechnologyName.Should().Be(techList[3].TechnologyName);
            result![4].Id.Should().Be(techList[4].Id);
        }

        [Fact]
        public async Task
          Controller_GetTechnologies_ShouldReturn200ResponseWithListOfTechnologiesLengthNotGreaterThan50_WhenRequestedPageSizeIsMoreThan50AndDatabaseSizeIsMoreThan50()
        {
            var techFake = new Faker<Technology>();

            List<Technology> techsFake = techFake.Generate(60);

            DbSeeder(db =>
            {
                db.Technologies.AddRange(techsFake);
                db.SaveChanges();
            });

            var resposne = await _httpClient.GetAsync("api/technologies?pageSize=60");

            var result = JsonConvert.DeserializeObject<List<TechnologyBasicResponse>>
                (await resposne.Content.ReadAsStringAsync());

            resposne.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

            result!.Count.Should().Be(50);
        }

        [Fact]
        public async Task
         Controller_GetTechnologies_ShouldReturn200ResponseWithRequestedTechnologiesLengthNotGreaterThan50_WhenDatabaseSizeIsSufficient()
        {
            var techFake = new Faker<Technology>();

            List<Technology> techsFake = techFake.Generate(60);

            DbSeeder(db =>
            {
                db.Technologies.AddRange(techsFake);
                db.SaveChanges();
            });

            var resposne = await _httpClient.GetAsync("api/technologies?pageSize=40");

            var result = JsonConvert.DeserializeObject<List<TechnologyBasicResponse>>
                (await resposne.Content.ReadAsStringAsync());

            resposne.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

            result!.Count.Should().Be(40);
        }
    }
}
