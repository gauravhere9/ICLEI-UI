using Microsoft.IdentityModel.Tokens;
using System.Net;

namespace WebApp.UI.Core.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IHostEnvironment _environment;
        private readonly ILogger<ExceptionMiddleware> _logger;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="next"></param>
        /// <param name="environment"></param>
        /// <param name="logger"></param>
        public ExceptionMiddleware(RequestDelegate next, IHostEnvironment environment, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _environment = environment;
            _logger = logger;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception Occured");

                await HandleExceptionAsync(httpContext, ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="ex"></param>
        /// <returns></returns>
        private async Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            context.Response.ContentType = "application/json";

            switch (ex)
            {
                case InvalidOperationException:
                    context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    context.Response.Redirect("/401-unauthorized");
                    break;
                case SecurityTokenValidationException:
                    context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    context.Response.Redirect("/401-unauthorized");
                    break;
                case SecurityTokenException:
                    context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    context.Response.Redirect("/401-unauthorized");
                    break;
                default:
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    context.Response.Redirect("/500-internal-server-error");
                    break;
            }
            return;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public static class ExceptionMiddlewareExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseExceptionMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ExceptionMiddleware>();
        }
    }
}
