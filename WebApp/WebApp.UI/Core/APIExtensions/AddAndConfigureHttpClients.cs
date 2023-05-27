using WebApp.DTOs.Auth.Login.Response;

namespace WebApp.UI.Core.APIExtensions
{
    public static partial class ApplicationServicesExtensions
    {
        public static IServiceCollection AddAndConfigureHttpClients(this IServiceCollection @this)
        {
            @this.AddHttpClient()
                .AddHttpClient("ICLEI API", (serviceProvider, httpClient) =>
                {
                    var httpContextAccessor = serviceProvider.GetService<HttpContextAccessor>();

                    SetAuthorizationHeaderToken(httpContextAccessor, httpClient);

                    httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
                })
                //.AddHttpMessageHandler(m =>
                //{
                //    var provider = @this.BuildServiceProvider();
                //    var tokenProvider = provider.GetRequiredService<TokenProvider>();

                //    return new ProtectedApiBearerTokenHandler(tokenProvider);

                //})
                ;

            return @this;
        }

        private static void SetAuthorizationHeaderToken(IHttpContextAccessor httpContextAccessor, HttpClient httpClient)
        {
            var authHeader = httpClient.DefaultRequestHeaders.Authorization;

            if (authHeader == null)
            {
                string token = GetAuthenticationTokenFromSession(httpContextAccessor);

                if (!string.IsNullOrEmpty(token))
                {
                    httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
                }
            }
        }

        private static string GetAuthenticationTokenFromSession(IHttpContextAccessor httpContextAccessor)
        {
            var tokenSession = httpContextAccessor.HttpContext.Session.GetString("X-Access-Token");

            if (!string.IsNullOrEmpty(tokenSession))
            {
                var tokenData = Newtonsoft.Json.JsonConvert.DeserializeObject<LoginResponseDto>(tokenSession);

                if (tokenData != null)
                {
                    return tokenData.Token;
                }
            }

            return string.Empty;
        }
    }
}
