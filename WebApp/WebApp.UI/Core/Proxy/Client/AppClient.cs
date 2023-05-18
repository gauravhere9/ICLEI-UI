using Polly;
using Polly.Retry;
using WebApp.DTOs.Auth.Login.Request;
using WebApp.DTOs.Auth.Login.Response;
using WebApp.DTOs.Auth.Password.Request;
using WebApp.DTOs.Auth.RefreshToken.Request;
using WebApp.DTOs.Auth.RefreshToken.Response;
using WebApp.DTOs.CompanyInfo.Request;
using WebApp.DTOs.CompanyInfo.Response;
using WebApp.Global.Options;
using WebApp.Global.Response;
using WebApp.Global.Shared.DTOs;
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

        private async Task<ApiResponse<T>> InvokeAPI<T>(object? request, string endpointUrl, HttpMethodTypes httpMethod)
        {
            _httpClient.Timeout = TimeSpan.FromSeconds(_apiOptions.RequestTimeOut);

            SetAuthorizationHeaderToken(_httpContextAccessor, _httpClient);

            var url = _apiOptions.BaseUrl + endpointUrl;

            HttpContent? httpContent = null;

            if (request != null)
            {
                httpContent = HttpContentHelper<T>.GetHttpRequestContentFromModel(request);
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

                return await HttpContentHelper<T>.GetModelFromHttpResponseContent(response.Content);
            });
        }

        private void SetAuthorizationHeaderToken(IHttpContextAccessor httpContextAccessor, HttpClient httpClient)
        {
            string token = GetAuthenticationTokenFromSession(httpContextAccessor);

            if (!string.IsNullOrEmpty(token))
            {
                httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
            }
        }

        private string GetAuthenticationTokenFromSession(IHttpContextAccessor httpContextAccessor)
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

        #endregion


        public async Task<ApiResponse<bool>> ChangePasswordAsync(ChangePasswordRequestDto requestDto)
        {
            var url = $"api/v1/auth/change-password";

            var response = await InvokeAPI<bool>(requestDto, url, HttpMethodTypes.Patch);

            return response;
        }

        public async Task<ApiResponse<bool>> ForgotPasswordAsync(AddForgotPasswordRequestDto requestDto)
        {
            var url = $"api/v1/auth/forgot-password";

            var response = await InvokeAPI<bool>(requestDto, url, HttpMethodTypes.Post);

            return response;
        }

        public async Task<ApiResponse<LoginResponseDto>> LoginAsync(LoginRequestDto requestDto)
        {
            var url = $"api/v1/auth";

            var response = await InvokeAPI<LoginResponseDto>(requestDto, url, HttpMethodTypes.Post);

            return response;
        }

        public async Task<ApiResponse<object>> LogoutAsync(RefreshTokenRequestDto requestDto)
        {
            var url = $"api/v1/auth/logout";

            var response = await InvokeAPI<object>(requestDto, url, HttpMethodTypes.Post);

            return response;
        }

        public async Task<ApiResponse<RefreshTokenResponseDto>> RefreshTokenAsync(RefreshTokenRequestDto requestDto)
        {
            var url = $"api/v1/auth/refresh-token";

            var response = await InvokeAPI<RefreshTokenResponseDto>(requestDto, url, HttpMethodTypes.Post);

            return response;
        }

        public async Task<ApiResponse<bool>> ResetPasswordAsync(ResetPasswordRequestDto requestDto)
        {
            var url = $"api/v1/auth/reset-password";

            var response = await InvokeAPI<bool>(requestDto, url, HttpMethodTypes.Patch);

            return response;
        }

        public async Task<ApiResponse<CompanyInfoResponseDto>> GetCompanyDetailsAsync()
        {
            var url = $"api/v1/company";

            var response = await InvokeAPI<CompanyInfoResponseDto>(null, url, HttpMethodTypes.Get);

            return response;
        }

        public async Task<ApiResponse<DropdownDto>> GetCompanyDropdownAsync()
        {
            var url = $"api/v1/company/dropdown";

            var response = await InvokeAPI<DropdownDto>(null, url, HttpMethodTypes.Get);

            return response;
        }

        public async Task<ApiResponse<object>> AddOrUpdateCompanyAsync(AddorUpdateCompanyInfoRequestDto requestDto)
        {
            var url = $"api/v1/company";

            var response = await InvokeAPI<object>(requestDto, url, HttpMethodTypes.Post);

            return response;
        }
    }
}
