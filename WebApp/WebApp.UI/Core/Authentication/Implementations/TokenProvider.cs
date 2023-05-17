using Microsoft.Extensions.Caching.Memory;
using WebApp.DTOs.Auth.Login.Response;
using WebApp.UI.Core.Authentication.Interfaces;

namespace WebApp.UI.Core.Authentication.Implementations
{
	public class TokenProvider : ITokenProvider
	{
		private readonly IMemoryCache _cache;
		private readonly IHttpContextAccessor _contextAccessor;
		public TokenProvider(IMemoryCache cache, IHttpContextAccessor contextAccessor)
		{
			_cache = cache ?? throw new ArgumentNullException(nameof(cache));
			_contextAccessor = contextAccessor;
		}
		public string GetTokenAsync()
		{
			//Check if cached token exist
			var key = "X-Access-Token";

			//string? cookieValue = _contextAccessor?.HttpContext?.Request?.Cookies[key];
			string? cookieValue = _contextAccessor.HttpContext.Session.GetString(key);

			if (!string.IsNullOrWhiteSpace(cookieValue))
			{
				var tokenData = Newtonsoft.Json.JsonConvert.DeserializeObject<LoginResponseDto>(cookieValue);

				if (tokenData != null)
				{
					return tokenData.Token;
				}
			}

			//_cache.TryGetValue(key, out LoginResponseDto responseDto);

			//if (responseDto != null)
			//{
			//    return responseDto.Token.ToString();
			//}

			return string.Empty;
		}
	}
}
