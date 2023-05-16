using Microsoft.AspNetCore.Mvc;
using WebApp.Global.Options;

namespace WebApp.UI.Controllers
{
	[Route("/users")]
	public class UsersController : BaseController
	{
		private readonly ApplicationOptions _applicationOptions;
		public UsersController(ApplicationOptions applicationOptions) : base(applicationOptions)
		{
			_applicationOptions = applicationOptions;
		}

		[HttpGet]
		[Route("")]
		public async Task<IActionResult> Index()
		{
			return await Task.Run(() => View());
		}
	}
}
