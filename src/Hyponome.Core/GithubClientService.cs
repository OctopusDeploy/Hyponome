using System;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Framework.Logging;
using Microsoft.Framework.OptionsModel;
using Octokit;

namespace Hyponome.Core
{
    public interface IGithubClientService
    {
        GithubOptions Options {get;}
        string CurrentState {get;}
        User CurrentUser {get;}
        PullRequest CurrentPullRequest {get;}
        void GenerateState();
        Task<User> SetCredentials(string token);
        Task<bool> IsCollaborator(string user);
        Task<IReadOnlyList<Organization>> GetOrganizations();
        Task<IReadOnlyList<Repository>> GetRepositories(string organization);
        Task<List<Issue>> GetPullRequests(string owner, string repo);
        Task<PullRequest> GetPullRequest(string owner, string repo, int number);
        Task<IReadOnlyList<PullRequestFile>> GetPullRequestFiles(string owner, string repo, int number);
        Task<PullRequestMerge> MergePullRequest(string owner, string repo, int number, MergePullRequest request);
        Task<IReadOnlyList<Milestone>> GetMilestones(string owner, string repo);
        Task<Milestone> GetMilestone(string owner, string repo, string milestone);
        Task<List<Issue>> GetIssuesInMilestone(string owner, string repo, string milestone);
    }
    
    public class GithubClientService : IGithubClientService
    {
        readonly ILogger<GithubClientService> logger;
        readonly GitHubClient githubClient;
        
        public GithubClientService(ILogger<GithubClientService> logger, IOptions<GithubOptions> optionsAccessor)
        {
            this.logger = logger;
            Options = optionsAccessor.Options;
            githubClient = new GitHubClient(new ProductHeaderValue("Hyponome"));
        }
        
        public GithubOptions Options {get;private set;}
        
        public string CurrentState {get;private set;}
        
        public User CurrentUser {get;private set;}
        
        public PullRequest CurrentPullRequest {get;private set;}
        
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
                return await githubClient.Repository.RepoCollaborators.IsCollaborator(Options.GithubOrganization, Options.GithubRepository, user);
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
        
        public async Task<IReadOnlyList<Repository>> GetRepositories(string organization)
        {
            return await githubClient.Repository.GetAllForOrg(organization);
        }
        
        public async Task<List<Issue>> GetPullRequests(string owner, string repo)
        {
            var issues = await githubClient.Issue.GetAllForRepository(owner, repo);
            var pulls = issues.Where(i => i.PullRequest != null);
            return pulls.ToList();
        }
        
        public async Task<PullRequest> GetPullRequest(string owner, string name, int number)
        {
            var pullRequest = CurrentPullRequest = await githubClient.PullRequest.Get(owner, name, number);
            return pullRequest;
        }
        
        public async Task<IReadOnlyList<PullRequestFile>> GetPullRequestFiles(string owner, string name, int number)
        {
            return await githubClient.PullRequest.Files(owner, name, number);
        }
        
        public async Task<PullRequestMerge> MergePullRequest(string owner, string repo, int number, MergePullRequest request)
        {
            return await githubClient.PullRequest.Merge(owner, repo, number, request);
        }
        
        public async Task<IReadOnlyList<Milestone>> GetMilestones(string owner, string repo)
        {
            return await githubClient.Issue.Milestone.GetAllForRepository(owner, repo);
        }
        
        public async Task<Milestone> GetMilestone(string owner, string repo, string milestone)
        {
            var milestones = await GetMilestones(owner, repo);
            var theMilestone = milestones.FirstOrDefault(m => m.Title == milestone);
            return theMilestone;
        }
        
        public async Task<List<Issue>> GetIssuesInMilestone(string owner, string repo, string milestone)
        {
            var issues = await githubClient.Issue.GetAllForRepository(
                owner, 
                repo, 
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
