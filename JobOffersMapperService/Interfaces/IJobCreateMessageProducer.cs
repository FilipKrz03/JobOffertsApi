using JobOffersApiCore.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobOffersMapperService.Interfaces
{
    public interface IJobCreateMessageProducer : IRabbitMessageProducer { }
    // Needed fod DI . Because two classes implement IRabbitMessageProducer Interface
}
