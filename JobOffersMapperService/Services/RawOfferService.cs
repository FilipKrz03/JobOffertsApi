using JobOffersApiCore.Dto;
using JobOffersMapperService.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobOffersMapperService.Services
{
    public class RawOfferService : IRawOfferService
    {
        public void HandleRawOffer(string body)
        {
            try
            {
                var offer = JsonConvert.DeserializeObject<JobOfferRaw>(body);
              
            
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        
        }
    }
}
