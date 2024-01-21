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
using static JobOffersMapperService.Props.RabbitMqJobProps;

namespace JobOffersMapperService.Services
{
    public class RawJobOfferService : IRawJobOfferService
    {

        private readonly IJobOffersBaseRepository _offersBaseRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<RawJobOfferService> _logger;
        private readonly IJobCreateMessageProducer _jobCreateMessageProducer;

        public RawJobOfferService(
            IJobOffersBaseRepository offersBaseRepository,
            IMapper mapper,
            ILogger<RawJobOfferService> logger,
            IJobCreateMessageProducer jobCreateMessageProducer
            )
        {
            _offersBaseRepository = offersBaseRepository;
            _mapper = mapper;
            _logger = logger;
            _jobCreateMessageProducer = jobCreateMessageProducer;
        }

        public async Task HandleRawOfferAsync(string body)
        {
            try
            {
                var offer = JsonConvert.DeserializeObject<JobOfferRaw>(body);

                if (offer == null) return;

                bool offerExist = await _offersBaseRepository.OfferExistAsync(offer);

                if (offerExist) return;

                var jobOfferBaseEntity = _mapper.Map<JobOfferRaw, JobOfferBase>(offer);

                _offersBaseRepository.Insert(jobOfferBaseEntity);

                await _offersBaseRepository.SaveChangesAsync();

                var processedJobOffer = _mapper.Map<JobOfferRaw, JobOfferProcessed>(offer, opts =>
                    opts.Items["Id"] = jobOfferBaseEntity.Id);

                _jobCreateMessageProducer.SendMessage(
                    JOB_OFFER_EXCHANGE,
                    JOB_CREATE_ROUTING_KEY,
                    processedJobOffer
                    );

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


