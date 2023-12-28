using Castle.Core.Logging;
using FirebaseAdmin.Auth;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using System.Text;
using System.Threading.Tasks;
using UsersService.Exceptions;
using UsersService.Middleware;

namespace UsersServiceTests.Middleware
{
    public class ExceptionHandlerMiddlewareTests
    {

        private readonly Mock<ILogger<ExceptionHandlerMiddleware>> _loggerMock;
        private readonly ExceptionHandlerMiddleware _middleware;
        private readonly HttpContext _context;

        public ExceptionHandlerMiddlewareTests()
        {
            _loggerMock = new();
            _middleware = new(_loggerMock.Object);
            _context = new DefaultHttpContext();
        }

        [Fact]
        public async Task Middleware_Should_CatchInvalidCredentialsException_WhenThrown()
        {
            string exMessage = "Invalid Credentials";

            var responseStream = new MemoryStream();
            _context.Response.Body = responseStream;

            var next = new RequestDelegate(_ => throw new InvalidCredentialsException(exMessage));
          
            await _middleware.InvokeAsync(_context, next);

            responseStream.Position = 0;

            _context.Response.StatusCode
                .Should()
                .Be(401);

            var responseMessage = await new StreamReader(responseStream).ReadToEndAsync();

            responseMessage
            .Should()
            .Be(exMessage);
        }

        [Fact]
        public async Task Middleware_Should_CatchInvalidRefreshTokenException_WhenThrown()
        {
            string exMessage = "Invalid Refresh Token";

            var responseStream = new MemoryStream();
            _context.Response.Body = responseStream;

            var next = new RequestDelegate(_ => throw new InvalidRefreshTokenException(exMessage));

            await _middleware.InvokeAsync(_context, next);

            responseStream.Position = 0;

            _context.Response.StatusCode
                .Should()
                .Be(403);

            var responseMessage = await new StreamReader(responseStream).ReadToEndAsync();

            responseMessage
            .Should()
            .Be(exMessage);
        }

        [Fact]
        public async Task Middleware_Should_CatchAnyOtherExceptionAndMappItTo500ServerResponse_WhenThrown()
        {
            var next = new RequestDelegate(_ => throw new AggregateException(""));

            await _middleware.InvokeAsync(_context, next);

            _context.Response.StatusCode
                .Should()
                .Be(500);
        }

        [Fact]
        public async Task Middleware_Should_CatchAnyOtherExceptionAndDoNotShowToUserErrorMessage_WhenThrown()
        {
            string exMessage = "Other ex";

            var responseStream = new MemoryStream();
            _context.Response.Body = responseStream;

            var next = new RequestDelegate(_ => throw new AggregateException(exMessage));

            await _middleware.InvokeAsync(_context, next);

            responseStream.Position = 0;

            var responseMessage = await new StreamReader(responseStream).ReadToEndAsync();

            responseMessage
                .Should()
                .NotBe(exMessage);
        }
    }
}
