using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UsersService.Profiles;

namespace UsersServiceTests.Profiles
{
    public class UserProfileTests
    {
        [Fact]
        public void MapperConfiguration_Should_BeValid()
        {
            var cfg = new MapperConfiguration(cfg => cfg.AddProfile<UserProfile>());

            cfg.AssertConfigurationIsValid();
        }
    }
}
