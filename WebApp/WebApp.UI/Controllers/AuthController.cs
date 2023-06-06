using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using WebApp.DTOs.Auth.Login.Request;
using WebApp.DTOs.Auth.Login.Response;
using WebApp.DTOs.Auth.Password.Request;
using WebApp.DTOs.Auth.RefreshToken.Request;
using WebApp.Global.Constants;
using WebApp.Global.Options;
using WebApp.UI.Core.Proxy.Client;

namespace WebApp.UI.Controllers
{
    public class AuthController : BaseController
    {
        private readonly ILogger<AuthController> _logger;
        private readonly IAppClient _appClient;
        private readonly ApplicationOptions _applicationOptions;
        private readonly AuthenticationOptions _authenticationOptions;

        public AuthController(ILogger<AuthController> logger, IAppClient appClient, ApplicationOptions applicationOptions, AuthenticationOptions authenticationOptions)
            : base(appClient, applicationOptions, authenticationOptions)
        {
            _logger = logger;
            _appClient = appClient;
            _applicationOptions = applicationOptions;
            _authenticationOptions = authenticationOptions;
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("", Name = "LoginDefault")]
        [Route("login", Name = "Login")]
        public async Task<IActionResult> Index()
        {
            return await Task.Run(() => View());
        }

        [HttpPost]
        [Route("", Name = "PostLogin")]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index([FromForm] LoginRequestDto requestDto)
        {
            if (ModelState.IsValid)
            {
                var response = await _appClient.LoginAsync(requestDto);

                if (response.Success)
                {
                    await StoreTokenInSession(response.Data);
                    return Redirect("/dashboard");
                }
                else
                {
                    StringBuilder sb = new StringBuilder();

                    foreach (var error in response.Errors)
                    {
                        sb.Append(error + ". ");
                    }

                    ViewBag.JavaScriptFunction = string.Format("ShowErrorSwal('{0}');", sb.ToString());
                }
            }
            else
            {
                ViewBag.JavaScriptFunction = string.Format("ShowErrorSwal('{0}');", ResponseStaticMessages.USERNAME_PASSWORD_MISMATCH);
            }

            return await Task.Run(() => View());
        }

        [HttpPost]
        [Route("logout", Name = "PostLogout")]
        public async Task<IActionResult> Logout()
        {
            var tokenSession = HttpContext.Session.GetString("X-Access-Token");

            if (!string.IsNullOrEmpty(tokenSession))
            {
                var tokenData = Newtonsoft.Json.JsonConvert.DeserializeObject<LoginResponseDto>(tokenSession);

                if (tokenData != null)
                {
                    RefreshTokenRequestDto requestDto = new RefreshTokenRequestDto()
                    {
                        UserId = tokenData.UserId,
                        RefreshToken = tokenData.RefreshToken,
                    };

                    var response = await _appClient.LogoutAsync(requestDto);

                    if (response.Success)
                    {
                        HttpContext.Session.SetString("X-Access-Token", string.Empty);

                        HttpContext.Session.Clear();

                        return Redirect("/");
                    }
                    else
                    {
                        StringBuilder sb = new StringBuilder();

                        foreach (var error in response.Errors)
                        {
                            sb.Append(error + ". ");
                        }

                        ViewBag.JavaScriptFunction = string.Format("ShowErrorSwal('{0}');", sb.ToString());
                    }
                }
            }

            ViewBag.JavaScriptFunction = string.Format("ShowErrorSwal('{0}');", ResponseStaticMessages.INVALID_REQUEST);

            return await Task.Run(() => View());
        }

        [Route("forgot-password", Name = "ForgotPasswordLoad")]
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPassword()
        {
            return await Task.Run(() => View());
        }

        [Route("forgot-password", Name = "ForgotPassword")]
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPassword([FromForm] AddForgotPasswordRequestDto requestDto)
        {
            if (ModelState.IsValid)
            {
                var result = await _appClient.ForgotPasswordAsync(requestDto);

                if (result.Success)
                {
                    ViewBag.JavaScriptFunction = string.Format("ShowSuccessSwal('{0}', '{1}');", result.Message, "/");
                }
                else
                {
                    StringBuilder sb = new StringBuilder();

                    foreach (var error in result.Errors)
                    {
                        sb.Append(error + ". ");
                    }

                    ViewBag.JavaScriptFunction = string.Format("ShowErrorSwal('{0}');", sb.ToString());
                }
            }
            else
            {
                ViewBag.JavaScriptFunction = string.Format("ShowErrorSwal('{0}');", "Please enter the username or email.");
            }

            return await Task.Run(() => View());
        }

        [Route("reset-password/{key}")]
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword([FromRoute] string key)
        {
            ViewBag.Key = key;
            return await Task.Run(() => View());
        }
    }
}