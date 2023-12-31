using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using UsersService.Exceptions;
using UsersService.Services;

namespace UsersServiceTests.Services
{
    public class ClaimServiceTests
    {
        
        private readonly Mock<IHttpContextAccessor> _httpContextAccesorMock;
        private readonly Mock<HttpContext> _httpContextMock;
        private readonly Mock<ClaimsPrincipal> _userMock;
        private readonly ClaimService _claimService;
        

        public ClaimServiceTests()
        {
            _httpContextMock = new();
            _httpContextAccesorMock = new();
            _userMock = new();

            _httpContextMock.Setup(x => x.User).Returns(_userMock.Object);
            _httpContextAccesorMock.Setup(x => x.HttpContext).Returns(_httpContextMock.Object);

            _claimService = new(_httpContextAccesorMock.Object);
        }

        [Fact]
        public void Service_Should_ThrowInvalidAccesTokenException_WhenIdToReturnEqualsNull()
        {
            _userMock.Setup(x => x.FindFirst(It.IsAny<string>()))
                 .Returns((Claim)null!);

            _claimService.Invoking(m => m.GetUserIdFromTokenClaim())
                .Should()
                .Throw<InvalidAccesTokenException>();
        }

        [Fact]
        public void Service_Should_ReturnClaimValue_WhenResultIsNotNull()
        {
            var claimToReturn = new Claim("user_id", "someValue");

            _userMock.Setup(x => x.FindFirst(It.IsAny<string>()))
                .Returns(claimToReturn);

            var result = _claimService.GetUserIdFromTokenClaim();

            result.Should().Be(claimToReturn.Value);
        }
    }
}
