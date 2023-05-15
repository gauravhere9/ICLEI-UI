using Microsoft.AspNetCore.Mvc;
using WebApp.Global.Options;

namespace WebApp.UI.Controllers
{
    public class DashboardController : BaseController
    {
        private readonly ApplicationOptions _applicationOptions;
        public DashboardController(ApplicationOptions applicationOptions) : base(applicationOptions)
        {
            _applicationOptions = applicationOptions;
        }

        [Route("dashboard")]
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return await Task.Run(() => View());
        }
    }
}
