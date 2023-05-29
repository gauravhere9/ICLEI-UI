using Microsoft.AspNetCore.Mvc;
using WebApp.Global.Options;
using WebApp.UI.Core.Proxy.Client;

namespace WebApp.UI.Controllers
{
    public class DashboardController : BaseController
    {
        private readonly IAppClient _appClient;
        private readonly ApplicationOptions _applicationOptions;
        private readonly AuthenticationOptions _authenticationOptions;
        public DashboardController(IAppClient appClient, ApplicationOptions applicationOptions, AuthenticationOptions authenticationOptions)
            : base(appClient, applicationOptions, authenticationOptions)
        {
            _appClient = appClient;
            _applicationOptions = applicationOptions;
            _authenticationOptions = authenticationOptions;
        }

        [Route("dashboard")]
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return await Task.Run(() => View());
        }
    }
}
