using Microsoft.AspNetCore.Mvc;
using WebApp.Global.Options;

namespace WebApp.UI.Controllers
{
    public class DashboardController : BaseController
    {
        private readonly ApplicationOptions _applicationOptions;
        private readonly AuthenticationOptions _authenticationOptions;
        public DashboardController(ApplicationOptions applicationOptions, AuthenticationOptions authenticationOptions) : base(applicationOptions, authenticationOptions)
        {
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
