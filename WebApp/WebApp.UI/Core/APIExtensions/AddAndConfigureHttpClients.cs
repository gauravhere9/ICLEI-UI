using WebApp.UI.Core.Authentication;
using WebApp.UI.Core.Authentication.Implementations;

namespace WebApp.UI.Core.APIExtensions
{
    public static partial class ApplicationServicesExtensions
    {
        public static IServiceCollection AddAndConfigureHttpClients(this IServiceCollection @this)
        {
            @this.AddHttpClient()
                .AddHttpClient("ICLEI API", c =>
                {
                    c.DefaultRequestHeaders.Add("Accept", "application/json");
                })
                .AddHttpMessageHandler(m =>
                {
                    var provider = @this.BuildServiceProvider();
                    var tokenProvider = provider.GetRequiredService<TokenProvider>();

                    return new ProtectedApiBearerTokenHandler(tokenProvider);

                });

            return @this;
        }
    }
}
