using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using StatusUpdateEventAPI.Helpers;

namespace StatusUpdateEventAPI.Helpers
{
    public class CustomExceptionFilter : IExceptionFilter
    {
        private readonly ILogger<CustomExceptionFilter> _logger;
        public CustomExceptionFilter(ILogger<CustomExceptionFilter> logger)
        {
            _logger = logger;
        }
        public void OnException(ExceptionContext context)
        {
            ErrorInformation errorInformation = null;
            var exceptionType = context.Exception.GetType();
            if (exceptionType == typeof(KeyNotFoundException))
            {
                errorInformation = new ErrorInformation()
                {
                    status = HttpStatusCode.NotFound,
                    developerMessage = context.Exception.Message,
                    moreInfo = string.Empty,
                    userMessage = context.Exception.Message,
                };
            }
            else if (context.Exception is DbUpdateException)
            {
                errorInformation = new ErrorInformation()
                {
                    status = HttpStatusCode.Conflict,
                    developerMessage = context.Exception.Message,
                    moreInfo = string.Empty,
                    userMessage = context.Exception.Message,
                };
            }
            else if (exceptionType == typeof(ArgumentException))
            {
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.NotFound;
                errorInformation = new ErrorInformation()
                {
                    status = HttpStatusCode.Conflict,
                    developerMessage = context.Exception.Message,
                    moreInfo = string.Empty,
                };
            }
            else if (exceptionType == typeof(ModelValidationException))
            {
                errorInformation = new ErrorInformation()
                {
                    status = HttpStatusCode.BadRequest,
                    developerMessage = context.Exception?.InnerException == null ? context.Exception.Message.ToString() : context.Exception?.InnerException?.Message,
                    errorCode = 0,
                    moreInfo = string.Empty,
                };
            }
            else if (exceptionType == typeof(Exception))
            {
                errorInformation = new ErrorInformation()
                {
                    status = HttpStatusCode.InternalServerError,
                    developerMessage = context.Exception.Message,
                    errorCode = 0,
                    moreInfo = string.Empty,
                };
            }
            else
            {
                errorInformation = new ErrorInformation()
                {
                    status = HttpStatusCode.InternalServerError,
                    developerMessage = context.Exception.Message,
                    moreInfo = string.Empty,
                };
            }
            context.HttpContext.Response.StatusCode = (int)errorInformation.status;
            context.HttpContext.Response.ContentType = "application/json";
            context.HttpContext.Response.WriteAsync(JsonConvert.SerializeObject(errorInformation)).Wait();
        }
    }
    public class ErrorInformation
    {
        public HttpStatusCode status { get; set; }
        public string developerMessage { get; set; }
        public string userMessage { get; set; }
        public int errorCode { get; set; }
        public string moreInfo { get; set; }
    }
}
