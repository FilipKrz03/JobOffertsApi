using AutoMapper;
using JobOffersService.Profiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobOfferServiceTests.Profiles
{
    public class JobOfferProfileTests
    {
        [Fact]
        public void MapperConfiguration_Should_BeValid()
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile<JobOfferProfile>());

            config.AssertConfigurationIsValid();
        }
    }
}
