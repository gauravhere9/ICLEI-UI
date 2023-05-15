using Polly;
using Polly.Retry;
using WebApp.DTOs.Auth.Login.Request;
using WebApp.DTOs.Auth.Password.Request;
using WebApp.DTOs.Auth.RefreshToken.Request;
using WebApp.Global.Options;
using WebApp.Global.Response;
using WebApp.UI.Core.Proxy.Helpers;
using static WebApp.Global.Constants.Enumurations;

namespace WebApp.UI.Core.Proxy.Client
{
    public class AppClient : IAppClient
    {
        #region Private Members
        private readonly AsyncRetryPolicy _retryPolicy;
        private readonly int _maxRetry = 3;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly HttpClient _httpClient;
        private readonly string _clientName = "ICLEI";
        private readonly ApiOptions _apiOptions;

        #endregion


        #region Constructor

        public AppClient(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor, ApiOptions apiOptions)
        {
            _apiOptions = apiOptions;
            _httpClient = httpClientFactory.CreateClient(_clientName);
            _httpClient.Timeout = TimeSpan.FromSeconds(_apiOptions.RequestTimeOut);
            _httpContextAccessor = httpContextAccessor;
            _retryPolicy = Policy.Handle<HttpRequestException>().WaitAndRetryAsync(_maxRetry, times => TimeSpan.FromMilliseconds(times * 200));

        }

        #endregion


        #region Private Methods

        private async Task<ApiResponse> InvokeAPI(object request, string endpointUrl, HttpMethodTypes httpMethod)
        {
            _httpClient.Timeout = TimeSpan.FromSeconds(_apiOptions.RequestTimeOut);

            var url = _apiOptions.BaseUrl + endpointUrl;

            HttpContent httpContent = null;

            if (request != null)
            {
                httpContent = HttpContentHelper.GetHttpRequestContentFromModel(request);
            }

            return await _retryPolicy.ExecuteAsync(async () =>
            {
                HttpResponseMessage response = httpMethod switch
                {
                    HttpMethodTypes.Get => await _httpClient.GetAsync(url),
                    HttpMethodTypes.Post => await _httpClient.PostAsync(url, httpContent),
                    HttpMethodTypes.Put => await _httpClient.PutAsync(url, httpContent),
                    HttpMethodTypes.Patch => await _httpClient.PatchAsync(url, httpContent),
                    HttpMethodTypes.Delete => await _httpClient.DeleteAsync(url),
                    _ => throw new InvalidOperationException("Unknown Http Method")
                };

                return await HttpContentHelper.GetModelFromHttpResponseContent(response.Content);
            });
        }

        #endregion


        public async Task<ApiResponse> ChangePassword(ChangePasswordRequestDto requestDto)
        {
            var url = $"api/v1/auth/change-password";

            var response = await InvokeAPI(requestDto, url, HttpMethodTypes.Patch);

            return response;
        }

        public async Task<ApiResponse> ForgotPassword(AddForgotPasswordRequestDto requestDto)
        {
            var url = $"api/v1/auth/forgot-password";

            var response = await InvokeAPI(requestDto, url, HttpMethodTypes.Post);

            return response;
        }

        public async Task<ApiResponse> Login(LoginRequestDto requestDto)
        {
            var url = $"api/v1/auth";

            var response = await InvokeAPI(requestDto, url, HttpMethodTypes.Post);

            return response;
        }

        public async Task<ApiResponse> Logout(RefreshTokenRequestDto requestDto)
        {
            var url = $"api/v1/auth/logout";

            var response = await InvokeAPI(requestDto, url, HttpMethodTypes.Post);

            return response;
        }

        public async Task<ApiResponse> RefreshToken(RefreshTokenRequestDto requestDto)
        {
            var url = $"api/v1/auth/refresh-token";

            var response = await InvokeAPI(requestDto, url, HttpMethodTypes.Post);

            return response;
        }

        public async Task<ApiResponse> ResetPassword(ResetPasswordRequestDto requestDto)
        {
            var url = $"api/v1/auth/reset-password";

            var response = await InvokeAPI(requestDto, url, HttpMethodTypes.Patch);

            return response;
        }
    }
}
