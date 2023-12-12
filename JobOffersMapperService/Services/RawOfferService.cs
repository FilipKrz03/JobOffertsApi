using AutoMapper;
using JobOffersApiCore.Dto;
using JobOffersApiCore.Enum;
using JobOffersApiCore.Interfaces;
using JobOffersMapperService.DbContexts;
using JobOffersMapperService.Entites;
using JobOffersMapperService.Interfaces;
using Microsoft.Extensions.Logging;
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

        private readonly IOffersBaseRepository _offersBaseRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<RawOfferService> _logger;

        public RawOfferService(IOffersBaseRepository offersBaseRepository , IMapper mapper , ILogger<RawOfferService> logger)
        {
            _offersBaseRepository = offersBaseRepository;
            _mapper = mapper;
            _logger = logger;
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

                _logger.LogInformation("Handle raw offer - New offer added to base db");
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occured in RawOfferService {ex}", ex);
            }
        }
    }
}
