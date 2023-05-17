using Microsoft.AspNetCore.Mvc;
using WebApp.Global.Options;

namespace WebApp.UI.Controllers
{
    [Route("/users")]
    public class UsersController : BaseController
    {
        private readonly ApplicationOptions _applicationOptions;
        private readonly AuthenticationOptions _authenticationOptions;
        public UsersController(ApplicationOptions applicationOptions, AuthenticationOptions authenticationOptions) : base(applicationOptions, authenticationOptions)
        {
            _applicationOptions = applicationOptions;
            _authenticationOptions = authenticationOptions;
        }

        [HttpGet]
        [Route("")]
        public async Task<IActionResult> Index()
        {
            return await Task.Run(() => View());
        }
    }
}
