using System;
using System.Threading.Tasks;
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
        Task<IReadOnlyList<PullRequest>> GetPullRequests(string owner, string repo);
        Task<PullRequest> GetPullRequest(string owner, string repo, int number);
        Task<IReadOnlyList<PullRequestFile>> GetPullRequestFiles(string owner, string repo, int number);
        Task<PullRequestMerge> MergePullRequest(string owner, string repo, int number, MergePullRequest request);
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
            return await githubClient.Repository.RepoCollaborators.IsCollaborator(Options.GithubOrganization, Options.GithubRepository, user);
        }
        
        public async Task<IReadOnlyList<Organization>> GetOrganizations()
        {
            return await githubClient.Organization.GetAllForCurrent();
        }
        
        public async Task<IReadOnlyList<Repository>> GetRepositories(string organization)
        {
            return await githubClient.Repository.GetAllForOrg(organization);
        }
        
        public async Task<IReadOnlyList<PullRequest>> GetPullRequests(string owner, string repo)
        {
            return await githubClient.PullRequest.GetAllForRepository(owner, repo);
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
    }
}
