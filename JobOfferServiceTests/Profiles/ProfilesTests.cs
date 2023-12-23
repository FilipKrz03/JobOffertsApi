using AutoMapper;
using JobOffersService.Profiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobOfferServiceTests.Profiles
{
    public class ProfilesTests
    {
        [Fact]
        public void MapperConfiguration_Should_BeValid()
        {       
            // Profiles need to be tested together because they are depending on each other

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<TechnologyProfile>();
                cfg.AddProfile<JobOfferProfile>();
            });

            config.AssertConfigurationIsValid();
        }
    }
}
