using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApp.DTOs.Auth.Login.Request;
using WebApp.Global.Options;
using WebApp.UI.Core.Proxy.Client;

namespace WebApp.UI.Controllers
{
    public class AuthController : BaseController
    {
        private readonly ILogger<AuthController> _logger;
        private readonly IAppClient _appClient;
        private readonly ApplicationOptions _applicationOptions;

        public AuthController(ILogger<AuthController> logger, IAppClient appClient, ApplicationOptions applicationOptions) : base(applicationOptions)
        {
            _logger = logger;
            _appClient = appClient;
            _applicationOptions = applicationOptions;
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("")]
        [Route("login")]
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
                var response = await _appClient.Login(requestDto);

                if (response.Success)
                {
                    //STORE THE TOKEN AND REFRESH TOKEN IN THE COOKIE

                    //var tokenDataString = Newtonsoft.Json.JsonConvert.SerializeObject(response.Data);

                    //HttpContext.Session.SetString("JwtToken", tokenDataString);

                    SetTokenInCookie(response.Data);

                    return Redirect("/dashboard");
                }
                else
                {
                    ViewBag.Errors = response.Errors;
                }
            }
            else
            {
                ViewBag.Errors = new List<string>() { "Invalid username or password" };
            }

            return await Task.Run(() => View(requestDto));
        }

        [Route("forgot-password")]
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPassword()
        {
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