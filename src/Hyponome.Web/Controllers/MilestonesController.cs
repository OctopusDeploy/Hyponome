using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Session;
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
	
	[RouteAttribute("{milestoneNumber}")]
	public async Task<ActionResult> GetMilestone(string milestoneNumber)
	{
		var milestone = await githubClientService.GetMilestone(githubOptions.GithubOrganization, githubOptions.GithubRepository, milestoneNumber); 
		var pulls = await githubClientService.GetIssuesInMilestone(githubOptions.GithubOrganization, githubOptions.GithubRepository, milestone.Number.ToString());
		System.Console.WriteLine("Found {0} pull requests in {1}", pulls.Count, milestone);
		return View("/Views/PullRequests/Index.cshtml", pulls);
	}
}