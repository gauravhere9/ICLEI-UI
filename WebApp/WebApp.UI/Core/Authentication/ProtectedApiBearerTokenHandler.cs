using System.Net.Http.Headers;
using WebApp.UI.Core.Authentication.Interfaces;

namespace WebApp.UI.Core.Authentication
{
    public class ProtectedApiBearerTokenHandler : DelegatingHandler
    {
        private readonly ITokenProvider _tokenProvider;
        public ProtectedApiBearerTokenHandler(ITokenProvider tokenProvider)
        {
            _tokenProvider = tokenProvider ?? throw new ArgumentNullException(nameof(tokenProvider));
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var accessToken = _tokenProvider.GetTokenAsync();

            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            return await base.SendAsync(request, cancellationToken);
        }
    }
}
