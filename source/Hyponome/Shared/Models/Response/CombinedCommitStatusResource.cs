using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Octokit;

namespace Hyponome.Shared.Models.Response
{
    public class CombinedCommitStatusResource
    {
        [JsonConstructor]
        public CombinedCommitStatusResource() {}

        public CombinedCommitStatusResource(string state, int totalCount, IReadOnlyList<CommitStatusResource> statuses)
        {
            State = state;
            TotalCount = totalCount;
            Statuses = statuses;
        }
        
        public string State { get; set; }
        public int TotalCount { get; set; }
        public IReadOnlyList<CommitStatusResource> Statuses { get; set; }
        
        public static implicit operator CombinedCommitStatusResource(CombinedCommitStatus status) =>
            new CombinedCommitStatusResource(status?.State.ToString(), status?.TotalCount ?? 0, status?.Statuses?.Select(s => (CommitStatusResource)s).ToArray());
    }

    public class CommitStatusResource
    {
        [JsonConstructor]
        public CommitStatusResource() {}
        
        public CommitStatusResource(string state, string context, string description)
        {
            State = state;
            Context = context;
            Description = description;
        }
        public string State { get; set; }
        public string Context { get; set; }
        public string Description { get; set; }
        
        public static implicit operator CommitStatusResource(CommitStatus status) =>
            new CommitStatusResource(status?.State.ToString(), status?.Context, status?.Description);
    }
}