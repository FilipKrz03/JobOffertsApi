﻿using JobOffersApiCore.Helpers;
using JobOffersService.Dto;
using JobOffersService.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JobOffersService.Controllers
{
    [Route("api/offers")]
    [ApiController]
    public class OffersController : ControllerBase
    {

        private readonly IJobOfferService _jobOffersService;

        public OffersController(IJobOfferService jobOffersService)
        {
            _jobOffersService = jobOffersService;
        }

        [HttpGet("{jobOfferId}")]
        public async Task<ActionResult<Response<JobOfferDetailResponse>>> GetJobOfferDetail(Guid jobOfferId)
        {
            var result = await _jobOffersService.GetJobOfferDetail(jobOfferId);

            return result.ErrorInfo.IsError ? StatusCode(result.ErrorInfo.StatusCode, result.ErrorInfo.ErrorMessage) :
                Ok(result.Value);
        }
    }
}
