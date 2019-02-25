using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using System.Net;
using Newtonsoft.Json;

namespace UserService {

    /**
     * This middleware was added to request pipeline
     * and cannot use await _next.Invoke(httpContext);
     * because it's a response middleware.
     * EndpointMiddlewareErrorTrapper can 
     * trap any exception in the API and response 
     * a custom message to client
     **/
    public class EndpointMiddlewareErrorTrapper {
        private const string WRONG_USER_NAME_OR_PASS = "User name or password incorrect";

        /**
         * Injections
         **/
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;

        public EndpointMiddlewareErrorTrapper(RequestDelegate next,
            ILogger<EndpointMiddlewareErrorTrapper> logger) {
            _next = next ?? throw new ArgumentNullException("Request Delegate");
            _logger = logger ?? throw new ArgumentException("Logger");
        }

        public async Task Invoke(HttpContext httpContext) {

            try {
                await _next(httpContext);
            } catch (Exception ex) when (ex is Exception) {
                _logger.LogDebug("Handle Exception Async");
                await HandleExceptionAsync(httpContext, ex);
            }

            // Call the next middleware delegate in the pipeline 
            //no next middleware because of http response, so comment _next.invoke
            // await _next.Invoke(httpContext);
        }

        /**
         * HandleExceptionAsync
         * handle exceptions throw all the API
         * @params object httpContext to make a response to client
         **/
        private static async Task HandleExceptionAsync(object context,
            Exception exception) {
            if (context == null) {
                throw new ArgumentNullException(nameof(context));
            }

            HttpContext httpContext = (HttpContext)context;
            HttpStatusCode code = HttpStatusCode.InternalServerError;

            //create error code and a json response with the error 
            //sending to client
            //also ignore nullable variables 
            var error = new { error = "error" };
            httpContext.Response.ContentType = "application/json";
            httpContext.Response.StatusCode = (int)code;
            await httpContext.Response.WriteAsync(JsonConvert.SerializeObject(error,
                new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore }));
        }
    }
}