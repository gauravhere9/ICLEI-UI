using Microsoft.AspNetCore.Mvc;
using WebApp.Global.Options;

namespace WebApp.UI.Controllers
{
    public class BaseController : Controller
    {
        private readonly ApplicationOptions _applicationOptions;
        public BaseController(ApplicationOptions applicationOptions)
        {
            _applicationOptions = applicationOptions;
        }

        private static readonly string keyTokenCookie = "X-Access-Token";

        protected void SetTokenInCookie(object? value)
        {
            var cookieOptions = new CookieOptions
            {
                Secure = true,
                HttpOnly = true,
                Expires = DateTime.UtcNow.AddMinutes(_applicationOptions.Cache.ExpirationMinutes),
                Path = "/"
            };

            string stringValue = Newtonsoft.Json.JsonConvert.SerializeObject(value, Newtonsoft.Json.Formatting.Indented);

            Response.Cookies.Append(keyTokenCookie, stringValue, cookieOptions);
        }
    }
}
