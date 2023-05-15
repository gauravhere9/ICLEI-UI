using WebApp.UI.Core.Proxy.Client;

namespace WebApp.UI.Core.APIExtensions
{
    public static class ApplicationServicesDependencyExtensions
    {
        public static IServiceCollection AddApplicationServicesBusinessDI(this IServiceCollection services)
        {
            #region BUSINESS LOGIC DEPENDENCY INJECTION
            services.AddTransient<IAppClient, AppClient>().AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            //services.AddTransient<CustomAuthorizationFilter>();
            #endregion

            return services;
        }
    }
}
