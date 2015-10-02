using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Authorization;
using Hyponome.Core;
using Octokit;

[RouteAttribute("")]
public class PullRequestsController : Controller
{
	readonly IGithubClientService githubClientService;
	readonly GithubOptions githubOptions;
	
	public PullRequestsController(IGithubClientService githubClientService)
	{
		this.githubClientService = githubClientService;
		this.githubOptions = githubClientService.Options;
	}
	
	[RouteAttribute("pulls")]
	public async Task<ActionResult> Index()
	{
		var pullRequests = await githubClientService.GetPullRequests(githubOptions.GithubOrganization, githubOptions.GithubRepository);
		Console.WriteLine("[Pulls] Found {0} open pull requests", pullRequests.Count);

		return View(pullRequests);
	}
	
	[RouteAttribute("pull/{number:int}")]
	public async Task<ActionResult> PullRequest(int number)
	{
		var pullRequest = await githubClientService.GetPullRequest(githubOptions.GithubOrganization, githubOptions.GithubRepository, number);
		if (pullRequest == null)
		{
			return HttpNotFound();
		}
		return await BuildView(pullRequest);
	}
	
	[AuthorizeAttribute]
	[HttpPostAttribute]
	[RouteAttribute("pull/{number:int}/merge")]
	public async Task<ActionResult> PullRequest(int number, [FromFormAttribute] MergePullRequest mergeRequest)
	{
		if (githubClientService.CurrentPullRequest == null)
		{
			Console.WriteLine("Current pull request is null");
			return RedirectToAction("Index");
		}
		
		if (!ModelState.IsValid)
		{
			Console.WriteLine("ModelState is invalid");
			TempData["Error"] = string.Join("\n", ModelState.Values);
			return await BuildView(githubClientService.CurrentPullRequest);
		}

		try
		{
			Console.WriteLine("Merge request sha: {0}, message: {1}", mergeRequest.Sha, mergeRequest.Message);
			var mergeResult = await githubClientService.MergePullRequest(githubOptions.GithubOrganization, githubOptions.GithubRepository, number, mergeRequest);
			if (!mergeResult.Merged)
			{
				Console.WriteLine("Merge failed: {0}", mergeResult.Message);
				TempData["Error"] = mergeResult.Message;
				return await BuildView(githubClientService.CurrentPullRequest);
			}
			TempData["Success"] = mergeResult.Message;
		}
		catch(ApiException aex)
		{
			Console.WriteLine("Merge threw an exception: {0}", aex.Message);
			var errors = new List<string> { aex.Message };
			if (aex.InnerException != null)
			{
				errors.Add(aex.InnerException.Message);
			}
			if (aex.ApiError.Errors.Count > 0)
			{
				foreach(var error in aex.ApiError.Errors)
				{
					errors.Add(error.Message);
				}
			}
			TempData["Error"] = string.Join("\n", errors);
			return await BuildView(githubClientService.CurrentPullRequest);
		}
		catch(Exception ex)
		{
			TempData["Error"] = ex.Message;
			return await BuildView(githubClientService.CurrentPullRequest);
		}
		
		Console.WriteLine("Merge successful");
		return RedirectToAction("Index");
	}
	
	private async Task<ActionResult> BuildView(PullRequest pullRequest)
	{
		ViewBag.IsMergeable = pullRequest.Mergeable.HasValue && pullRequest.Mergeable.Value;
		var pullRequestFiles = await githubClientService.GetPullRequestFiles(githubOptions.GithubOrganization, githubOptions.GithubRepository, pullRequest.Number);
		ViewBag.Files = pullRequestFiles;
		ViewBag.IsCollaborator = string.IsNullOrEmpty(HttpContext.User.Identity.Name) ? false : await githubClientService.IsCollaborator(HttpContext.User.Identity.Name);
		
		return View("PullRequest", pullRequest);
	}
}