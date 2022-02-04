using InterviewTask.Logic.Exceptions;
using InterviewTask.Logic.Models.Response;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace InterviewTask.WepApi.Middleware
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
                await SendHttpResponse((int)HttpStatusCode.BadRequest, context, linkInvalidException);
            }
            catch (NotFoundException notFoundException)
            {
                await SendHttpResponse((int)HttpStatusCode.NotFound, context, notFoundException);
            }
            catch (HttpRequestException ex)
            {
                await SendHttpResponse((int)HttpStatusCode.BadRequest, context, ex);
            }
            catch (Exception ex)
            {
                await SendHttpResponse((int)HttpStatusCode.InternalServerError, context, ex);
            }
        }

        private async Task SendHttpResponse(int statusCode, HttpContext context, Exception exception)
        {
            var jsonObject = JsonConvert.SerializeObject(new ExceptionResponse
            {
                Message = exception.Message
            });

            var response = context.Response;

            response.ContentType = "application/json";
            response.StatusCode = statusCode;

            await response.WriteAsync(jsonObject);
        }
    }
}