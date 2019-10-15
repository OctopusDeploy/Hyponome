using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Octokit;

namespace Hyponome.Server.Services
{
    public class GitHubClientServiceStub : IGitHubClientService
    {
        public GitHubClientServiceStub(IOptions<GitHubOptions> optionsAccessor)
        {
            Options = optionsAccessor.Value;
        }

        GitHubOptions Options { get; }

        public string CurrentState => throw new System.NotImplementedException();

        public User CurrentUser => throw new System.NotImplementedException();

        public PullRequest CurrentPullRequest => throw new System.NotImplementedException();

        public Task<IEnumerable<Issue>> GetPullRequests()
        {
            var issues = ReadResource<IEnumerable<Issue>>("issues.json");
            var pulls = issues.Where(i => i.PullRequest != null);
            return Task.FromResult(pulls);
        }

        public Task<PullRequest> GetPullRequest(int number)
        {
            var pullRequest = ReadResource<PullRequest>(number.ToString(), "pullrequest.json");
            return Task.FromResult(pullRequest);
        }

        public void GenerateState()
        {
            throw new System.NotImplementedException();
        }

        public Task<User> SetCredentials(string token)
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> IsCollaborator(string user)
        {
            throw new System.NotImplementedException();
        }

        public Task<IReadOnlyList<Organization>> GetOrganizations()
        {
            throw new System.NotImplementedException();
        }

        public Task<IReadOnlyList<Repository>> GetRepositories()
        {
            throw new System.NotImplementedException();
        }

        public Task<IReadOnlyList<PullRequestFile>> GetPullRequestFiles(int number)
        {
            var files = ReadResource<IReadOnlyList<PullRequestFile>>(number.ToString(), "pullrequest-files.json");
            return Task.FromResult(files);
        }

        public Task<PullRequestMerge> MergePullRequest(int number, MergePullRequest request)
        {
            throw new System.NotImplementedException();
        }

        public Task<IReadOnlyList<Milestone>> GetMilestones()
        {
            throw new System.NotImplementedException();
        }

        public Task<Milestone> GetMilestone(string milestone)
        {
            throw new System.NotImplementedException();
        }

        public Task<List<Issue>> GetIssuesInMilestone(string milestone)
        {
            throw new System.NotImplementedException();
        }

        T ReadResource<T>(params string[] paths)
        {
            return new Octokit.Internal.SimpleJsonSerializer()
                .Deserialize<T>(File.ReadAllText(
                    Path.Combine(Directory.GetCurrentDirectory(), "sample-data", Path.Combine(paths))));
        }
    }
}