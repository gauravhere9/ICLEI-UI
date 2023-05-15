using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Primitives;
using WebApp.Global.Options;

namespace WebApp.UI.Core.APIExtensions
{
    public static class ApplicationBuilderExtensions
    {
        public static void UseForwardHeadersWithBasePath(this IApplicationBuilder @this)
        {
            var applicationOptions = @this.ApplicationServices.GetService<ApplicationOptions>();

            @this.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });

            @this.UsePathBase(applicationOptions.Hosting.BasePath);

            @this.Use((context, next) =>
            {
                if (applicationOptions.Hosting.UseHttps)
                {
                    context.Request.Scheme = "https";
                }

                context.Request.PathBase = new PathString(applicationOptions.Hosting.BasePath);

                //Setting the response header
                SetResponseHeaders(context);

                return next();
            });
        }

        private static void SetResponseHeaders(HttpContext context)
        {
            context.Response.Headers.Add("Content-Type", new StringValues("application/json"));
            context.Response.Headers.Add("x-content-type-options", new StringValues("nosniff"));
            context.Response.Headers.Add("Referrer-Policy", new StringValues("no-referrer"));
            context.Response.Headers.Add("x-frame-options", new StringValues("DENY"));
            context.Response.Headers.Add("Content-Security-Policy", new StringValues("default-src 'self';"));
            context.Response.Headers.Add("Content-Encoding", new StringValues("Gzip"));
            context.Response.Headers.Add("X-Permitted-Cross-Domain-Policies", new StringValues("none"));
            context.Response.Headers.Add("x-xss-protection", new StringValues("1, mode=block"));

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="this"></param>
        public static void UseAppSettings(this IApplicationBuilder @this)
        {
            var options = @this.ApplicationServices.GetService<ApplicationOptions>();

            if (!string.IsNullOrWhiteSpace(options.Hosting.BasePath))
            {
                @this.UseForwardHeadersWithBasePath();
            }

            if (options.Hosting.UseHttps)
            {
                @this.UseHsts();
                @this.UseHttpsRedirection();
            }
        }
    }
}
