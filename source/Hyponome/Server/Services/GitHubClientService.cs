using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Hyponome.Server.Utils;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Octokit;

namespace Hyponome.Server.Services
{
    public interface IGitHubClientService
    {
        string CurrentState { get; }
        User CurrentUser { get; }
        PullRequest CurrentPullRequest { get; }
        void GenerateState();
        Task<User> SetCredentials(string token);
        Task<bool> IsCollaborator(string user);
        Task<IReadOnlyList<Organization>> GetOrganizations();
        Task<IReadOnlyList<Repository>> GetRepositories();
        Task<IReadOnlyList<(PullRequest, CombinedCommitStatus)>> GetPullRequests();
        Task<(PullRequest, IReadOnlyList<PullRequestFile>, IReadOnlyList<PullRequestReview>, CombinedCommitStatus)> GetPullRequest(int number);
        Task<IReadOnlyList<PullRequestFile>> GetPullRequestFiles(int number);
        Task<IReadOnlyList<PullRequestReview>> GetPullRequestReviews(int number);
        Task<CombinedCommitStatus> GetPullRequestStatuses(string sha);
        Task<PullRequestMerge> MergePullRequest(int number, MergePullRequest request);
        Task<IReadOnlyList<Milestone>> GetMilestones();
        Task<Milestone> GetMilestone(string milestone);
        Task<List<Issue>> GetIssuesInMilestone(string milestone);
    }

    public class GitHubClientService : IGitHubClientService
    {
        readonly ILogger<GitHubClientService> logger;
        readonly GitHubClient githubClient;
        private readonly string version = typeof(GitHubClientService).Assembly.GetFileVersion();

        public GitHubClientService(ILogger<GitHubClientService> logger, IOptions<GitHubOptions> optionsAccessor)
        {
            this.logger = logger;
            Options = optionsAccessor.Value;
            githubClient = new GitHubClient(new ProductHeaderValue("Hyponome", version));
        }

        GitHubOptions Options { get; }

        public string CurrentState { get; private set; }

        public User CurrentUser { get; private set; }

        public PullRequest CurrentPullRequest { get; private set; }

        public void GenerateState()
        {
            CurrentState = Guid.NewGuid().ToString("N");
        }

        public async Task<User> SetCredentials(string token)
        {
            githubClient.Credentials = new Credentials(token);
            CurrentUser = await githubClient.User.Current();
            return CurrentUser;
        }

        public async Task<bool> IsCollaborator(string user)
        {
            try
            {
                return await githubClient.Repository.Collaborator.IsCollaborator(Options.OrganizationName, Options.RepositoryName, user);
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<IReadOnlyList<Organization>> GetOrganizations()
        {
            return await githubClient.Organization.GetAllForCurrent();
        }

        public async Task<IReadOnlyList<Repository>> GetRepositories()
        {
            return await githubClient.Repository.GetAllForOrg(Options.OrganizationName);
        }

        public async Task<IReadOnlyList<(PullRequest, CombinedCommitStatus)>> GetPullRequests()
        {
            var issues = await githubClient.Issue.GetAllForRepository(Options.OrganizationName, Options.RepositoryName);
            IEnumerable<(PullRequest PullRequest, IReadOnlyList<PullRequestFile>, IReadOnlyList<PullRequestReview>, CombinedCommitStatus Status)> pullRequestDetails = issues.Where(i => i.PullRequest != null).Select(pr => GetPullRequest(pr.Number).Result);
            
            return pullRequestDetails.Select(pr => (pr.PullRequest, pr.Status)).ToList();
        }

        public async Task<(PullRequest, IReadOnlyList<PullRequestFile>, IReadOnlyList<PullRequestReview>, CombinedCommitStatus)> GetPullRequest(int number)
        {
            var pullRequest = await githubClient.PullRequest.Get(Options.OrganizationName, Options.RepositoryName, number);
            var files = await GetPullRequestFiles(number);
            var reviews = await GetPullRequestReviews(number);
            var statuses = await GetPullRequestStatuses(pullRequest.Head.Sha);
                
            return (pullRequest, files, reviews, statuses);
        }

        public async Task<IReadOnlyList<PullRequestFile>> GetPullRequestFiles(int number)
        {
            return await githubClient.PullRequest.Files(Options.OrganizationName, Options.RepositoryName, number);
        }

        public async Task<IReadOnlyList<PullRequestReview>> GetPullRequestReviews(int number)
        {
            return await githubClient.PullRequest.Review.GetAll(Options.OrganizationName, Options.RepositoryName, number);
        }

        public async Task<CombinedCommitStatus> GetPullRequestStatuses(string sha)
        {
            return await githubClient.Repository.Status.GetCombined(Options.OrganizationName, Options.RepositoryName, sha); 
        }

        public async Task<PullRequestMerge> MergePullRequest(int number, MergePullRequest request)
        {
            return await githubClient.PullRequest.Merge(Options.OrganizationName, Options.RepositoryName, number, request);
        }

        public async Task<IReadOnlyList<Milestone>> GetMilestones()
        {
            return await githubClient.Issue.Milestone.GetAllForRepository(Options.OrganizationName, Options.RepositoryName);
        }

        public async Task<Milestone> GetMilestone(string milestone)
        {
            var milestones = await GetMilestones();
            var theMilestone = milestones.FirstOrDefault(m => m.Title == milestone);
            return theMilestone;
        }

        public async Task<List<Issue>> GetIssuesInMilestone(string milestone)
        {
            var issues = await githubClient.Issue.GetAllForRepository(
                Options.OrganizationName,
                Options.RepositoryName,
                new RepositoryIssueRequest
                {
                    Milestone = milestone
                }
            );
            var pulls = issues.Where(i => i.PullRequest != null);
            return pulls.ToList();
        }
    }
}
