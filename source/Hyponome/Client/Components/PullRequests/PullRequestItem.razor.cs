using Hyponome.Shared.Models.Response;
using Microsoft.AspNetCore.Components;

namespace Hyponome.Client.Components.PullRequests
{
    public class PullRequestItemBase : ComponentBase
    {
        [Parameter]
        public PullRequestResource PullRequest { get; set; }
    }
}
