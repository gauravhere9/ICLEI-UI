using Microsoft.AspNetCore.Mvc;
using System.Text;
using WebApp.DTOs.CompanyInfo.Request;
using WebApp.Global.Constants;
using WebApp.Global.Options;
using WebApp.UI.Core.Proxy.Client;

namespace WebApp.UI.Controllers
{
    [Route("/company-profile")]
    public class CompanyProfileController : BaseController
    {
        private readonly ILogger<AuthController> _logger;
        private readonly IAppClient _appClient;
        private readonly ApplicationOptions _applicationOptions;
        private readonly AuthenticationOptions _authenticationOptions;
        public CompanyProfileController(ILogger<AuthController> logger, IAppClient appClient, ApplicationOptions applicationOptions,
            AuthenticationOptions authenticationOptions) : base(appClient, applicationOptions, authenticationOptions)
        {
            _logger = logger;
            _appClient = appClient;
            _applicationOptions = applicationOptions;
            _authenticationOptions = authenticationOptions;
        }

        [HttpGet]
        [Route("")]
        public async Task<IActionResult> Index()
        {
            AddorUpdateCompanyInfoRequestDto? requestDto = null;

            var result = await _appClient.GetCompanyDetailsAsync();

            if (result.Success)
            {
                if (result.Data != null)
                {
                    requestDto = new AddorUpdateCompanyInfoRequestDto()
                    {
                        Name = result.Data.Name,
                        Email = result.Data.Email,
                        Phone = result.Data.Phone,
                        Address = result.Data.Address,
                        Website = result.Data.Website ?? string.Empty,
                        Fax = result.Data.Fax ?? string.Empty
                    };
                }
            }

            return await Task.Run(() => View(requestDto));
        }

        [HttpPost]
        [Route("", Name = "PostCompanyProfile")]
        public async Task<IActionResult> Index([FromForm] AddorUpdateCompanyInfoRequestDto requestDto)
        {
            if (ModelState.IsValid)
            {
                var response = await _appClient.AddOrUpdateCompanyAsync(requestDto);

                if (response.Success)
                {
                    ViewBag.JavaScriptFunction = string.Format("ShowSuccessSwal('{0}', '/company-profile');", response.Message);
                }
                else
                {
                    StringBuilder sb = new StringBuilder();

                    if (response.Errors != null)
                    {
                        foreach (var error in response.Errors)
                        {
                            sb.Append(error + ". ");
                        }
                    }
                    else
                    {
                        sb.Append("Something went wrong. Try Again Later.");
                    }

                    ViewBag.JavaScriptFunction = string.Format("ShowErrorSwal('{0}');", sb.ToString());
                }
            }
            else
            {
                ViewBag.JavaScriptFunction = string.Format("ShowErrorSwal('{0}');", ResponseStaticMessages.INVALID_INPUT);
            }

            return await Task.Run(() => View());
        }
    }
}
