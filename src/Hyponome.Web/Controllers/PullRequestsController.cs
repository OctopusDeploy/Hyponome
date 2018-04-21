using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Hyponome.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Octokit;

namespace Hyponome.Web.Controllers
{
    [Route("pulls")]
    public class PullRequestsController : Controller
    {
        readonly IGitHubClientService githubClientService;
        readonly GitHubOptions githubOptions;

        public PullRequestsController(IGitHubClientService githubClientService)
        {
            this.githubClientService = githubClientService;
            this.githubOptions = githubClientService.Options;
        }

        [Route("")]
        public async Task<ActionResult> Index()
        {
            var pullRequests = await githubClientService.GetPullRequests(githubOptions.OrganizationName, githubOptions.RepositoryName);
            Console.WriteLine("[Pulls] Found {0} open pull requests", pullRequests.Count);

            return View(pullRequests);
        }

        [Route("{number:int}")]
        public async Task<ActionResult> PullRequest(int number)
        {
            var pullRequest = await githubClientService.GetPullRequest(githubOptions.OrganizationName, githubOptions.RepositoryName, number);
            if (pullRequest == null)
            {
                return NotFound();
            }
            return await BuildView(pullRequest);
        }
        private async Task<ActionResult> BuildView(PullRequest pullRequest)
        {
            ViewBag.IsMergeable = pullRequest.Mergeable.HasValue && pullRequest.Mergeable.Value;
            var pullRequestFiles = await githubClientService.GetPullRequestFiles(githubOptions.OrganizationName, githubOptions.RepositoryName, pullRequest.Number);
            ViewBag.Files = pullRequestFiles;
            ViewBag.IsCollaborator = string.IsNullOrEmpty(HttpContext.User.Identity.Name) ? false : await githubClientService.IsCollaborator(HttpContext.User.Identity.Name);

            return View("PullRequest", pullRequest);
        }
    }
}