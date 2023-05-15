using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.UI.Controllers
{
    [Route("")]
    public class ErrorController : Controller
    {
        [Route("401-unauthorized")]
        [AllowAnonymous]
        public async Task<IActionResult> UnauthorizedAccess()
        {
            return await Task.Run(() => View());
        }

        [Route("404-not-found")]
        [AllowAnonymous]
        public async Task<IActionResult> ResourceNotFound()
        {
            return await Task.Run(() => View());
        }

        [Route("500-internal-server-error")]
        [AllowAnonymous]
        public async Task<IActionResult> InternalServerError()
        {
            return await Task.Run(() => View());
        }
    }
}
