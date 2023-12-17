using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobOffersApiCore.Common
{
    public class ErrorInfo
    {
        public bool IsError { get; set; } = false;

        public int StatusCode { get; set; }

        public string ErrorMessage { get; set; } = string.Empty;   

        public void SetError(int statusCode , string errorMessage)
        {
            IsError = true;
            StatusCode = statusCode;
            ErrorMessage = errorMessage;
        }
    }
}
