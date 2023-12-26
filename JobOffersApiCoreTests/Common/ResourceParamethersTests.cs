using JobOffersApiCore.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobOffersApiCoreTests.Common
{
    public class ResourceParamethersTests
    {

        [Theory]
        [InlineData(60)]
        [InlineData(100)]
        [InlineData(1000)]
        public void PageSize_Should_BeNoGreaterThan50EventIfUserTrySetThisToGreaterNumber(int pageSizeToSet)
        {
            ResourceParamethers resourceParamethers = new() { PageSize = pageSizeToSet };

            Assert.Equal(50 , resourceParamethers.PageSize);
        }

        [Fact]
        public void PageNumber_Should_Equal1_WhenPageNumberNotSetted()
        {
            ResourceParamethers resourceParamethers = new();

            Assert.Equal(1, resourceParamethers.PageNumber);
        }

        [Fact]
        public void SortOrder_Should_EqualAsc_WhenSortOrderNotSetted()
        {
            ResourceParamethers resourceParamethers = new();

            Assert.Equal("asc", resourceParamethers.SortOrder);
        }

        [Fact]
        public void PageSize_Should_Equal30_WhenPageSizeNotSetted()
        {
            ResourceParamethers resourceParamethers = new();

            Assert.Equal(30, resourceParamethers.PageSize);
        }
    }
}
