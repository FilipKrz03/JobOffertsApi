using AutoMapper;
using JobOffersService.Interfaces;
using JobOffersService.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobOfferServiceTests.Services
{
    public class OfferServiceTests
    {

        private readonly Mock<IOfferRepository> _offerRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly OfferService _offerService;

        public OfferServiceTests()
        {
            _mapperMock = new();
            _offerRepositoryMock = new();
            _offerService = new(_offerRepositoryMock.Object , _mapperMock.Object);
        }
    }
}
