using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebScrapperService.Interfaces;

namespace WebScrapperService.Services
{
    public class OffersService : IOffersService 
    {
        private readonly IEnumerable<IScrapperService> _scrapperServices;

        public OffersService(IEnumerable<IScrapperService> scrapperServices)
        {
            _scrapperServices = scrapperServices;
        }

        public void HandleOffersCreateAndUpdate()
        {
            foreach(var service in _scrapperServices)
            {
                //service.ScrapOfferts();
            }
        }
    }
}
