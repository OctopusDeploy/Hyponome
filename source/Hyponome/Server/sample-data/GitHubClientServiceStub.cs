using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Hyponome.Shared.Models.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Octokit;
using Octokit.Internal;

namespace Hyponome.Server.Services
{
    public class GitHubClientServiceStub : IGitHubClientService
    {
        private readonly ILogger<GitHubClientServiceStub> logger;
        readonly GitHubClient githubClient;
        public GitHubClientServiceStub(ILogger<GitHubClientServiceStub> logger, IOptions<GitHubOptions> optionsAccessor)
        {
            this.logger = logger;
            Options = optionsAccessor.Value;
            githubClient = new GitHubClient(new ProductHeaderValue("Hyponome", "0.0.0-local"));
            githubClient.Credentials = new Credentials(Environment.GetEnvironmentVariable("OCTOKIT_OAUTHTOKEN"));
        }

        GitHubOptions Options { get; }

        public string CurrentState => throw new System.NotImplementedException();

        public User CurrentUser => throw new System.NotImplementedException();

        public PullRequest CurrentPullRequest => throw new System.NotImplementedException();

        public async Task<IReadOnlyList<(PullRequest, CombinedCommitStatus)>> GetPullRequests()
        {
            var issues = ReadResource<IReadOnlyList<Issue>>("issues.json");
            if (issues is null)
            {
                issues = await githubClient.Issue.GetAllForRepository(Options.OrganizationName, Options.RepositoryName);
                WriteResource(issues, "issues.json");
            }
            var pulls = issues.Where(i => i.PullRequest != null).Select(pr => GetPullRequest(pr.Number).Result);
            return (IReadOnlyList<(PullRequest, CombinedCommitStatus)>)pulls.Select(p => (p.Item1, p.Item4)).ToList();
        }

        public async Task<(PullRequest, IReadOnlyList<PullRequestFile>, IReadOnlyList<PullRequestReview>, CombinedCommitStatus)> GetPullRequest(int number)
        {
            var pullRequest = ReadResource<PullRequest>(number.ToString(), "pullrequest.json");
            if (pullRequest is null)
            {
                pullRequest =
                    await githubClient.PullRequest.Get(Options.OrganizationName, Options.RepositoryName, number);
                WriteResource(pullRequest, number.ToString(), "pullrequest.json");
            }
            return (
                pullRequest, 
                GetPullRequestFiles(number).Result, 
                GetPullRequestReviews(number).Result, 
                GetPullRequestStatuses(pullRequest?.Head?.Sha).Result
            );
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

        public async Task<IReadOnlyList<PullRequestFile>> GetPullRequestFiles(int number)
        {
            var files = ReadResource<IReadOnlyList<PullRequestFile>>(number.ToString(), "pullrequest-files.json");
            if (files is null)
            {
                files = await githubClient.PullRequest.Files(Options.OrganizationName, Options.RepositoryName, number);
                WriteResource(files, number.ToString(), "pullrequest-files.json");
            }
            return files;
        }

        public async Task<IReadOnlyList<PullRequestReview>> GetPullRequestReviews(int number)
        {
            var reviews = ReadResource<IReadOnlyList<PullRequestReview>>(number.ToString(), "pullrequest-reviews.json");
            if (reviews is null)
            {
                reviews = await githubClient.PullRequest.Review.GetAll(Options.OrganizationName, Options.RepositoryName,
                    number);
                WriteResource(reviews, number.ToString(), "pullrequest-reviews.json");
            }
            return reviews;
        }

        public async Task<CombinedCommitStatus> GetPullRequestStatuses(string sha)
        {
            var status = ReadResource<CombinedCommitStatus>("CommitStatus", $"{sha}.json");
            if (status is null)
            {
                status = await githubClient.Repository.Status.GetCombined(Options.OrganizationName,
                    Options.RepositoryName, sha);
                WriteResource(status, "CommitStatus", $"{sha}.json");
            }
            return status;
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

        private T ReadResource<T>(params string[] paths) where T : class
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), "sample-data", Path.Combine(paths));
            try
            {
                return new SimpleJsonSerializer()
                    .Deserialize<T>(File.ReadAllText(
                        path));
            }
            catch(Exception e)
            {
                logger.LogError(e, $"An error occurred while reading {path}");
                return null;
            }
        }

        private void WriteResource<T>(T resource, params string[] paths) where T : class
        {
            var fullPath = Path.Combine(Directory.GetCurrentDirectory(), "sample-data", Path.Combine(paths));
            var path = Path.GetDirectoryName(fullPath);
            try
            {
                if (!Directory.Exists(path)) Directory.CreateDirectory(path);
                
                File.WriteAllText(
                    fullPath, 
                    new SimpleJsonSerializer().Serialize(resource)
                );
            }
            catch(Exception e)
            {
                logger.LogError(e, $"An error occurred while writing to {fullPath}");
            }
        }
    }
}