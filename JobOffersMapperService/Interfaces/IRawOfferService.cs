using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobOffersMapperService.Interfaces
{
    public interface IRawOfferService
    {
        public void HandleRawOffer(string body);
    }
}
