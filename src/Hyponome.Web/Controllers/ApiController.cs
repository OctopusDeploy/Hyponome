using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Authorization;
using Hyponome.Core;

[AuthorizeAttribute]
[RouteAttribute("api")]
public class ApiController : Controller
{
	readonly IGithubClientService githubClientService;
	
	public ApiController(IGithubClientService githubClientService)
	{
		this.githubClientService = githubClientService;
	}
	
	[RouteAttribute("")]
	public string Index()
	{
		return "Welcome to the API!";
	}
}