using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Threading.Tasks;
using Hyponome.Server.Services;
using Hyponome.Shared.Models.Response;
using Microsoft.AspNetCore.Mvc;
using Octokit;

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
            (PullRequest PullRequest, CombinedCommitStatus Status)[] pullRequests = (await githubClientService.GetPullRequests()).ToArray();
            
            return Ok(pullRequests.Select(pr => PullRequestResource.FromResponseModel(pr.PullRequest, null, null, pr.Status)));
        }

        [Route("{number:int}")]
        public async Task<IActionResult> PullRequest(int number)
        {
            var (pullRequest, files, reviews, statuses) = await githubClientService.GetPullRequest(number);
            if (pullRequest == null)
            {
                return NotFound();
            }

            return Ok(PullRequestResource.FromResponseModel(pullRequest, reviews, files, statuses));
        }
    }
}
