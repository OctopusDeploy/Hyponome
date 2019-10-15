using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hyponome.Server.Services;
using Hyponome.Shared.Models.Response;
using Microsoft.AspNetCore.Mvc;

namespace Hyponome.Server.Controllers
{
    [Route("api/pulls")]
    [ApiController]
    public class PullRequestsController : ControllerBase
    {
        readonly IGitHubClientService githubClientService;

        public PullRequestsController(IGitHubClientService githubClientService)
        {
            this.githubClientService = githubClientService;
        }

        [Route("")]
        public async Task<IActionResult> Index()
        {
            var pullRequests = (await githubClientService.GetPullRequests()).ToArray();
            Console.WriteLine("[Pulls] Found {0} open pull requests!", pullRequests.Count());
            
            return Ok(pullRequests.Select(pr => new IssueResource(pr.Number, pr.Title, pr.CreatedAt, pr.User, pr.Comments, pr.Milestone)));
        }

        [Route("{number:int}")]
        public async Task<IActionResult> PullRequest(int number)
        {
            var pullRequest = await githubClientService.GetPullRequest(number);
            if (pullRequest == null)
            {
                return NotFound();
            }

            var files = await githubClientService.GetPullRequestFiles(number);
            return Ok(PullRequestResource.FromModel(pullRequest, files));
        }
    }
}
