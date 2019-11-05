using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Octokit;

namespace Hyponome.Shared.Models.Response
{
    public class PullRequestResource : IssueResource
    {
        [JsonConstructor]
        public PullRequestResource() { }

        public PullRequestResource(
            int number, 
            string title, 
            string url,
            UserResource user, 
            int commits,
            GitReferenceResource @base, 
            GitReferenceResource head,
            int changedFiles,
            int additions,
            int deletions,
            int comments,
            IReadOnlyList<LabelResource> labels,
            IReadOnlyList<PullRequestReviewResource> reviews,
            IReadOnlyList<PullRequestFileResource> files)
        {
            Number = number;
            Title = title;
            Url = url;
            User = user;
            Commits = commits;
            Base = @base;
            Head = head;
            ChangedFiles = changedFiles;
            Additions = additions;
            Deletions = deletions;
            Comments = comments;
            Labels = labels;
            Reviews = reviews;
            Files = files;
        }

        public string Url { get; set; }
        public int Commits { get; set; }
        public GitReferenceResource Base { get; set; }
        public GitReferenceResource Head { get; set; }
        public int ChangedFiles { get; set; }
        public int Additions { get; set; }
        public int Deletions { get; set; }
        public IReadOnlyList<PullRequestReviewResource> Reviews { get; set; }
        public IReadOnlyList<PullRequestFileResource> Files { get; set; }

        public static PullRequestResource FromResponseModel(PullRequest pullRequest, IReadOnlyList<PullRequestReview> reviews, IReadOnlyList<PullRequestFile> files) => 
            new PullRequestResource(
                pullRequest.Number,
                pullRequest.Title,
                pullRequest.HtmlUrl,
                pullRequest.User,
                pullRequest.Commits,
                pullRequest.Base,
                pullRequest.Head,
                pullRequest.ChangedFiles,
                pullRequest.Additions,
                pullRequest.Deletions,
                pullRequest.Comments,
                pullRequest.Labels?.Select(l => new LabelResource(l.Name, l.Color, l.Description)).ToList(),
                reviews?.Select(r => new PullRequestReviewResource(r.State.StringValue)).ToList(),
                files?.Select(f => new PullRequestFileResource(f.FileName, f.Sha, f.Patch)).ToList());
    }
}
