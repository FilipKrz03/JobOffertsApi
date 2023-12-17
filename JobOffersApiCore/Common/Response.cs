using JobOffersApiCore.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobOffersApiCore.Helpers
{
    public class Response<T>
    {
        public ErrorInfo ErrorInfo { get; set; }
        public T? Value { get; set; }

        public Response()
        {
            ErrorInfo = new();
        }

        public Response<T> SetError(int statusCode , string errorMessage)
        {
            ErrorInfo.SetError(statusCode, errorMessage);

            return this;
        }
    }
}
