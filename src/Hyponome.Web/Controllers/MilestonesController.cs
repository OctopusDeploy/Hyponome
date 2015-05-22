using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using Hyponome.Core;

[RouteAttribute("[controller]")]
public class MilestonesController : Controller
{
	readonly IGithubClientService githubClientService;
	readonly GithubOptions githubOptions;
	
	public MilestonesController(IGithubClientService githubClientService)
	{
		this.githubClientService = githubClientService;
		this.githubOptions = githubClientService.Options;	
	}
	
	[RouteAttribute("")]
	public async Task<ActionResult> Index()
	{
		var milestones = await githubClientService.GetMilestones(githubOptions.GithubOrganization, githubOptions.GithubRepository);
		return View(milestones);
	}
	
	[RouteAttribute("{milestone}")]
	public async Task<ActionResult> GetMilestone(string milestone)
	{
		return View();
	}
}