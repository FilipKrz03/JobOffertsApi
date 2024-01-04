using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UsersService.Profiles;

namespace UsersServiceTests.Profiles
{
    public class ProfileTests
    {
        [Fact]
        public void MapperConfiguration_Should_BeValid()
        {
            // All profiles tested because they are depending on each other

            var cfg = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<UserProfile>();
                cfg.AddProfile<JobOfferProfile>();
                cfg.AddProfile<TechnologyProfile>();
                cfg.AddProfile<PagedListProfile>();
            });

            cfg.AssertConfigurationIsValid();
        }
    }
}
