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

        [Fact]
        public void Converter_Should_ReturnProperSeniorityEnumValue_WhenSeniorityStringEqualsJunior()
        {
            JobOfferRaw offerRaw = new("", "", "", "", "Junior", Enumerable.Empty<string>(), "", "Inavlid salary string");

            JobOfferProcessed processed = _converter.Convert(offerRaw, default!, default!);

            Assert.Equal(Seniority.Junior, processed.Seniority);
        }

        [Fact]
        public void Converter_Should_ReturnProperSeniorityEnumValue_WhenSeniorityStringEqualsMid()
        {
            JobOfferRaw offerRaw = new("", "", "", "", "Mid", Enumerable.Empty<string>(), "", "Inavlid salary string");

            JobOfferProcessed processed = _converter.Convert(offerRaw, default!, default!);

            Assert.Equal(Seniority.Mid, processed.Seniority);
        }

        [Fact]
        public void Converter_Should_ReturnProperSeniorityEnumValue_WhenSeniorityStringEqualsSenior()
        {
            JobOfferRaw offerRaw = new("", "", "", "", "Senior", Enumerable.Empty<string>(), "", "Inavlid salary string");

            JobOfferProcessed processed = _converter.Convert(offerRaw, default!, default!);

            Assert.Equal(Seniority.Senior, processed.Seniority);
        }

        [Fact]
        public void Converter_Should_ReturnUnkownSeniorityValue_WhenSeniorityStringIsNotRecognized()
        {
            JobOfferRaw offerRaw = new("", "", "", "", "unkown seniority", Enumerable.Empty<string>(), "", "Inavlid salary string");

            JobOfferProcessed processed = _converter.Convert(offerRaw, default!, default!);

            Assert.Equal(Seniority.Unknown, processed.Seniority);
        }
    }
}
