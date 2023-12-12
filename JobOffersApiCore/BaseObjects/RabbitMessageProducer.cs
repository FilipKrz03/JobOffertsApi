using JobOffersApiCore.Interfaces;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobOffersApiCore.BaseConfigurations
{
    public class RabbitMessageProducer : RabbitBaseConfig , IRabbitMessageProducer
    {
        public RabbitMessageProducer(string connectionUri, string clientProvidedName , bool asyncMode) : 
            base(connectionUri, clientProvidedName , asyncMode) { }

        public void SendMessage<T>(string exchange, string routingKey, T? message)
        {
            if(message == null)
            {
                _chanel.BasicPublish(exchange, routingKey, null , null);
                return;
            }

            var jsonString = JsonConvert.SerializeObject(message);

            byte[] messageBodyBytes = Encoding.UTF8.GetBytes(jsonString);

            _chanel.BasicPublish(exchange, routingKey, null, messageBodyBytes);
        }

        public void SendMessage(string exchange, string routingKey)
        {
            _chanel.BasicPublish(exchange, routingKey, null, null);
        }

        public void CloseConnection()
        {
            _chanel.Close();
            _connection.Close();
        }
    }
}
