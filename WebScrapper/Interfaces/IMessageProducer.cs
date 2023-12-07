using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;
using WebScrapperService.Dto;

namespace WebScrapperService.Interfaces
{
    public interface IJobOfferMessageProducer
    {
        void SendMessage (JobOffer message);      
        void CloseConnection();
    }
}
