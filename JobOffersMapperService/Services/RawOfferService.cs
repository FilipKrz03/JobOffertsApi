using AutoMapper;
using JobOffersApiCore.Dto;
using JobOffersApiCore.Enum;
using JobOffersApiCore.Interfaces;
using JobOffersMapperService.DbContexts;
using JobOffersMapperService.Entites;
using JobOffersMapperService.Interfaces;
using JobOffersMapperService.Props;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobOffersMapperService.Services
{
    public class RawOfferService : IRawOfferService
    {

        private readonly IOffersBaseRepository _offersBaseRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<RawOfferService> _logger;
        private readonly IRabbitMessageProducer _jobCreateMessageProducer;

        public RawOfferService(IOffersBaseRepository offersBaseRepository , IMapper mapper , 
            ILogger<RawOfferService> logger , IRabbitMessageProducer jobCreateMessageProducer)
        {
            _offersBaseRepository = offersBaseRepository;
            _mapper = mapper;
            _logger = logger;
            _jobCreateMessageProducer = jobCreateMessageProducer;
        }

        public async Task HandleRawOffer(string body)
        {
            try
            {
                var offer = JsonConvert.DeserializeObject<JobOfferRaw>(body);

                if (offer == null) return;

                bool offerExist = await _offersBaseRepository.OfferExistAsync(offer);

                if (offerExist) return;
                 
                var jobOfferBaseEntity = _mapper.Map<JobOfferRaw, JobOfferBase>(offer);

                await _offersBaseRepository.Insert(jobOfferBaseEntity);

                var processedJobOffer = _mapper.Map<JobOfferRaw, JobOfferProcessed>(offer);

                _jobCreateMessageProducer.SendMessage
                    (RabbitMqJobCreateProps.JOB_OFFER_EXCHANGE , RabbitMqJobCreateProps.JOB_CREATE_ROUTING_KEY , processedJobOffer);

                _logger.LogInformation
                    ("Handle raw offer - New offer added to base db and create job offer event sended");
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occured in RawOfferService {ex}", ex);
            }
        }
    }
}


