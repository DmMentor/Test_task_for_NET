using InterviewTask.Logic.Exceptions;
using InterviewTask.Logic.Models.Response;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Threading.Tasks;

namespace InterviewTask.WepApi.Middlewares
{
    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (InputLinkInvalidException linkInvalidException)
            {
                await SendHttpResponse(context, linkInvalidException);
            }
            catch (Exception ex)
            {
                await SendHttpResponse(context, ex);
            }
        }

        private async Task SendHttpResponse(HttpContext context, Exception exception)
        {
            var jsonObject = JsonConvert.SerializeObject(new ExceptionResponse
            {
                Message = exception.Message
            });

            var response = context.Response;

            response.ContentType = "application/json";
            response.StatusCode = (int)HttpStatusCode.BadRequest;

            await response.WriteAsync(jsonObject);
        }
    }
}