using JobOffersApiCore.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobOffersApiCoreTests.Common
{
    public class ResponseTests
    {
        [Fact]
        public void Response_ReturnError_Should_ReturnObjectWithProperErrorInfoSetted()
        {
            int statusCode = 400;
            string erroMessage = "Fatal error";

            var returnedResponse = Response<string>.ReturnError(statusCode, erroMessage);

            Assert.True(returnedResponse.ErrorInfo.IsError);
            Assert.Equal(statusCode, returnedResponse.ErrorInfo.StatusCode);
            Assert.Equal(erroMessage, returnedResponse.ErrorInfo.ErrorMessage);
        }

        [Fact]
        public void Response_ReturnValue_Should_ReturnObjectWithPropertValueAndErrorInfoSettedAsFalse()
        {
            string value = "Proper value as string";

            var returnedResponse = Response<string>.ReturnValue(value);

            Assert.False(returnedResponse.ErrorInfo.IsError);
            Assert.Equal(returnedResponse.Value, value);
        }
    }
}
