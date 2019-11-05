using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Octokit;

namespace Hyponome.Shared.Models.Response
{
    public class IssueResource
    {
        [JsonConstructor]
        public IssueResource() { }

        public IssueResource(int number, string title, DateTimeOffset created, UserResource user, int comments, MilestoneResource milestone, IReadOnlyList<LabelResource> labels)
        {
            Number = number;
            Title = title;
            Created = created;
            User = user;
            Comments = comments;
            Milestone = milestone;
            Labels = labels;
        }

        public int Number { get; set; }
        public string Title { get; set; }
        public DateTimeOffset Created { get; set; }
        public UserResource User { get; set; }
        public int Comments { get; set; }
        public MilestoneResource Milestone { get; set; }
        public IReadOnlyList<LabelResource> Labels { get; set; }

        public static IssueResource FromResponseModel(Issue pr)
        {
            return new IssueResource(
                pr.Number, 
                pr.Title, 
                pr.CreatedAt, 
                pr.User, 
                pr.Comments, 
                pr.Milestone,
                pr.Labels.Select(l => new LabelResource(l.Name, l.Color, l.Description)).ToList()
            );
        }
    }
}
