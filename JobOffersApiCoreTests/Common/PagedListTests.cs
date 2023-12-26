using JobOffersApiCore.Common;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobOffersApiCoreTests.Common
{
    public class PagedListTests
    {

        [Theory]
        [InlineData(123 , 7 , 18)]
        [InlineData(122, 24 , 6)]
        [InlineData(117, 35, 4)]
        [InlineData(42, 6 , 7)]
        public void TotalPagesProperty_Should_BeCorectlyCalculated(int totalCount , int pageSize , int expectedPageCount)
        {
            PagedList<string> pagedList = new(GetHelperItems(), pageSize, 1, totalCount);

            Assert.Equal(pagedList.TotalPages , expectedPageCount);
        }

        [Theory]
        [InlineData(1, false)]
        [InlineData(2, true)]
        [InlineData(100, true)]
        [InlineData(4342, true)]
        public void HasPreviousProperty_Should_BeCorrectlyCalculated(int pageNumber, bool hasPrevious)
        {
            PagedList<string> pagedList = new(GetHelperItems(), 1, pageNumber, 1);

            Assert.Equal(pagedList.HasPrevious, hasPrevious);
        }

        [Theory]
        [InlineData(10 , 20 , 10 , false )]
        [InlineData(3 , 56 , 23 , false)]
        [InlineData(5 , 150 , 30 , false)]
        [InlineData(3 , 300 , 50 , true)]
        public void HasNextProperty_Should_BeCorrectlyCalculated
            (int pageNumber , int totalCount , int pageSize , bool expectedValue)
        {
            PagedList<string> pagedList = new(GetHelperItems(), pageSize, pageNumber, totalCount);

            Assert.Equal(expectedValue, pagedList.HasNext);
        }

        private List<string> GetHelperItems()
        {
            return new() { "simple", "test", "items" };
        }
    }
}
