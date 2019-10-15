using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Octokit;

namespace Hyponome.Shared.Models.Response
{
    public class PullRequestResource
    {
        [JsonConstructor]
        public PullRequestResource() { }

        public PullRequestResource(
            int number, 
            string title, 
            UserResource user, 
            int commits,
            GitReferenceResource @base, 
            GitReferenceResource head,
            int changedFiles,
            int additions,
            int deletions,
            int comments,
            IReadOnlyList<PullRequestFileResource> files)
        {
            Number = number;
            Title = title;
            User = user;
            Commits = commits;
            Base = @base;
            Head = head;
            ChangedFiles = changedFiles;
            Additions = additions;
            Deletions = deletions;
            Comments = comments;
            Files = files;
        }

        public int Number { get; set; }
        public string Title { get; set; }
        public UserResource User { get; set; }
        public int Commits { get; set; }
        public GitReferenceResource Base { get; set; }
        public GitReferenceResource Head { get; set; }
        public int ChangedFiles { get; set; }
        public int Additions { get; set; }
        public int Deletions { get; set; }
        public int Comments { get; set; }
        public IReadOnlyList<PullRequestFileResource> Files { get; set; }

        public static PullRequestResource FromModel(PullRequest pullRequest, IReadOnlyList<PullRequestFile> files) => 
            new PullRequestResource(
                pullRequest.Number,
                pullRequest.Title,
                pullRequest.User,
                pullRequest.Commits,
                pullRequest.Base,
                pullRequest.Head,
                pullRequest.ChangedFiles,
                pullRequest.Additions,
                pullRequest.Deletions,
                pullRequest.Comments,
                files.Select(f => new PullRequestFileResource(f.FileName, f.Sha)).ToList());
    }
}
