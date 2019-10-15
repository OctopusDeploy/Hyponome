using Newtonsoft.Json;
using Octokit;

namespace Hyponome.Shared.Models.Response
{
    public class MilestoneResource
    {
        [JsonConstructor]
        public MilestoneResource() { }

        public MilestoneResource(string title)
        {
            Title = title;
        }

        public string Title { get; set; }

        public static implicit operator MilestoneResource(Milestone milestone) => new MilestoneResource(milestone.Title);
    }
}
