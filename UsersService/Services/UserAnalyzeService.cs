using System.Diagnostics.CodeAnalysis;
using UsersService.Entities;
using UsersService.Interfaces.RepositoriesInterfaces;
using UsersService.Interfaces.ServicesInterfaces;

namespace UsersService.Services
{
    public class UserAnalyzeService : IUserAnalyzeService
    {

        private readonly IJobOfferRepository _jobOfferRepository;
        private readonly IUserRepository _userRepository;

        public UserAnalyzeService(IJobOfferRepository jobOfferRepository, IUserRepository userRepository)
        {
            _jobOfferRepository = jobOfferRepository;
            _userRepository = userRepository;
        }

        public async Task LetUsersKnowAboutNewMatchingOffers()
        {
            DateTime tresholdDate = DateTime.UtcNow - TimeSpan.FromHours(240);

            var newlyCreatedOffers =
                await _jobOfferRepository.GetJobOffersFromTresholdDate(tresholdDate);

            var allUsers = await _userRepository.GetAllUsersAsync();

            // For tests 

            List<Technology> techs = new List<Technology>()
            {
                new Technology(){TechnologyName = "C#"} ,
                new Technology(){TechnologyName = "Angular"}
            };

            List<Technology> techs2 = new List<Technology>()
            {
                new Technology(){TechnologyName = "Java"} ,
                new Technology(){TechnologyName = "React.Js"}
            };

            List<Technology> techs3 = new List<Technology>()
            {
                new Technology(){TechnologyName = "JavaScript"} ,
                new Technology(){TechnologyName = "Nest.js"} ,
                new Technology(){TechnologyName = "Next.js"} ,
            };

            List<Technology> techs4 = new List<Technology>()
            {
                new Technology(){TechnologyName = "Nest.js"} ,
                new Technology(){TechnologyName = "Next.js"} ,
                new Technology(){TechnologyName = "JavaScript"} ,
            };

            List<User> fakeUsers = new List<User>()
            {
                new User() {Technologies = techs},
                new User(){Technologies = techs2} ,
                new User(){Technologies = techs3} ,
                new User(){Technologies = techs4} ,
                new User(){Technologies = techs4} ,
                new User(){Technologies = techs3} ,
                new User(){Technologies = techs2} ,
                new User(){Technologies = techs},
                new User(){Technologies = techs4} ,
                new User(){Technologies = techs} ,
                new User(){Technologies = techs3} ,
                new User(),
                new User(),
            };


            // good way ? 
            var groupedUsers = fakeUsers.GroupBy
                (u => string.Join(",", u.Technologies.OrderBy(t => t.TechnologyName).Select(t => t.TechnologyName)));

            foreach (var group in groupedUsers)
            {
                var groupTechnologyNames = group.Key.Split(",");

                var matchingOffers = newlyCreatedOffers
                    .Where(offer => offer.Technologies
                        .Any(tech => groupTechnologyNames.Contains(tech.TechnologyName))).ToList();

                foreach (var user in group)
                {
                    user.JobOffers = matchingOffers;
                }

                var userList = group.ToList();

                // Send event to send email (body should be : userList + mattchingOffers)
            }
        }
    }
}
