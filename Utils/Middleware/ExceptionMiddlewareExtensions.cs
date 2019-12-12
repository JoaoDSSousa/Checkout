using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Utils.Middleware
{
    //A global error handler that will prevent the stacktrace from being written to the output.
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate next;
        private readonly ILogger logger;

        public ErrorHandlingMiddleware(RequestDelegate next, ILogger logger)
        {
            this.next = next;
            this.logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await next(context);
            }
            //Capture all the exceptions
            catch (Exception ex)
            {
                //Rewrite the response
                await SetErrorResponseAsync(context, ex);
                
                //Log the error
                logger.LogError(ex, "An unexpected error occurred.");

                //The exception is not re-thrown on purpose.
            }
        }

        private Task SetErrorResponseAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            return context.Response.WriteAsync("An unexpected error occurred.");
        }
    }
}
