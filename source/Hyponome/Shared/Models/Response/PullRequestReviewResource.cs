using Newtonsoft.Json;

namespace Hyponome.Shared.Models.Response
{
    public class PullRequestReviewResource
    {
        [JsonConstructor]
        public PullRequestReviewResource() {}

        public PullRequestReviewResource(string state)
        {
            State = state;
        }
        
        public string State { get; set; }
    }
}