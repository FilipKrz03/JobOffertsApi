using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobOffersApiCore.Exceptions
{
    public class ResourceAlreadyExistException : Exception
    {
        public ResourceAlreadyExistException(string message)
            : base(message) { } 
    }
}
