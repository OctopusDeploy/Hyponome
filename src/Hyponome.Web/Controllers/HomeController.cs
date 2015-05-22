using Microsoft.AspNet.Mvc;
using Hyponome.Core;

namespace Hyponome.Controllers
{
	[RouteAttribute("")]
	public class HomeController : Controller
	{
		readonly IGithubClientService githubClientService;
		readonly GithubOptions githubOptions;
		
		public HomeController(IGithubClientService githubClientService)
		{
			this.githubClientService = githubClientService;
			this.githubOptions = githubClientService.Options;
		}

		[RouteAttribute("")]
		public ActionResult Index()
		{
			return RedirectToAction("Index", "PullRequests");
		}
	}
}