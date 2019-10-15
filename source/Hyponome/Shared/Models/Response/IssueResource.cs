using System;
using Newtonsoft.Json;
using Octokit;

namespace Hyponome.Shared.Models.Response
{
    public class IssueResource
    {
        [JsonConstructor]
        public IssueResource() { }

        public IssueResource(int number, string title, DateTimeOffset created, UserResource user, int comments, MilestoneResource milestone)
        {
            Number = number;
            Title = title;
            Created = created;
            User = user;
            Comments = comments;
            Milestone = milestone;
        }

        public int Number { get; set; }
        public string Title { get; set; }
        public DateTimeOffset Created { get; set; }
        public UserResource User { get; set; }
        public int Comments { get; set; }
        public MilestoneResource Milestone { get; set; }
    }
}
