using Microsoft.OpenApi.Exceptions;
using System.Diagnostics;
using System.Net;
using System.Text.Json;
using talabat.APIs.Errors;

namespace talabat.APIs.MiddleWares
{
    public class ExceptionMiddleWares
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleWares> _logger;
        private readonly IHostEnvironment _environment;

        public ExceptionMiddleWares(RequestDelegate Next , ILogger<ExceptionMiddleWares> logger , IHostEnvironment environment )
        {
            _next = Next;
            _logger = logger;
            _environment = environment;
        }


        //Invoke Function


        public async Task InvokeAsync(HttpContext context)
        {

            try
            {
                await _next.Invoke(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex , ex.Message);

                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int) HttpStatusCode.InternalServerError;

 

                var Response = _environment.IsDevelopment() ? new ApiExceptionResponse(500, ex.Message, ex.StackTrace.ToString()) : new ApiExceptionResponse((int)HttpStatusCode.InternalServerError);
                var JsonResponse = JsonSerializer.Serialize(Response);
                context.Response.WriteAsync(JsonResponse);


            }


        }


    }
}
