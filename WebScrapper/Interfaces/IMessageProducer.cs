using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace WebScrapperService.Interfaces
{
    public interface IMessageProducer<T>
    {
        void SendMessage (T message);      
        void CloseConnection();
    }
}
