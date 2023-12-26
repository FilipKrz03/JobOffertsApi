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

        public static Response<T> ReturnError(int statusCode, string errorMessage)
        {
            var response = new Response<T>();

            response.ErrorInfo.SetError(statusCode, errorMessage);

            return response;
        }

        public static Response<T> ReturnValue(T value)
        {
            var response = new Response<T>();

            response.Value = value;

            return response;
        }
    }
}
