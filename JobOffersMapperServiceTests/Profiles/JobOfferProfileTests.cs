using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using JobOffersMapperService.Profiles;

namespace JobOffersMapperServiceTests.Profiles
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
