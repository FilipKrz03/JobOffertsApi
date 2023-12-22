using AutoMapper;
using JobOffersApiCore.Dto;
using JobOffersApiCore.Enum;
using JobOffersMapperService.Profiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobOffersMapperServiceTests.Profiles
{
    public class JobOfferRawConverterTests
    {
        private readonly JobOfferRawConverter _converter;

        public JobOfferRawConverterTests()
        {
            _converter = new JobOfferRawConverter();    
        }

        [Fact]
        public void Converter_Should_ReturnProperEarningFields_WhenSalaryStringIsProper()
        {
            JobOfferRaw offerRaw = new("", "", "", "", "" , Enumerable.Empty<string>() , "", "19 000-21 000 zł");

            JobOfferProcessed processed = _converter.Convert(offerRaw, default!, default!);

            Assert.True(processed.EarningsFrom == 19000);
            Assert.True(processed.EarningsTo == 21000);
        }

        [Fact]
        public void Converter_Should_ReturnNullableEarningFields_WhenSalayStringIsInvalid()
        {
            JobOfferRaw offerRaw = new("", "", "", "", "", Enumerable.Empty<string>(), "", "Inavlid salary string");

            JobOfferProcessed processed = _converter.Convert(offerRaw, default!, default!);

            Assert.True(processed.EarningsFrom == null);
            Assert.True(processed.EarningsTo == null);
        }

        [Theory]
        [InlineData("junior" , Seniority.Junior)]
        [InlineData("mid" , Seniority.Mid)]
        [InlineData("senior" , Seniority.Senior)]
        [InlineData("Junior  ", Seniority.Junior)]
        [InlineData("mId  ", Seniority.Mid)]
        [InlineData(" Senior", Seniority.Senior)]
        [InlineData("Not recognaizable seniority" , Seniority.Unknown)]
        public void Converter_Should_ReturnProperSeniorityValue_WhenMappingFromStringSeniorityToEnumSeniority
            (string stringSeniority , Seniority expectedSeniority)
        {
            JobOfferRaw offerRaw = new("", "", "", "", stringSeniority , Enumerable.Empty<string>(), "", "Inavlid salary string");

            JobOfferProcessed processed = _converter.Convert(offerRaw, default!, default!);

            Assert.Equal(expectedSeniority, processed.Seniority);
        }
    }
}
