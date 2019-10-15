using Hyponome.Shared.Models.Response;
using Microsoft.AspNetCore.Components;

namespace Hyponome.Client.Components.PullRequests
{
    public class PullRequestListItemBase : ComponentBase
    {
        [Parameter]
        public IssueResource PullRequest { get; set; }
    }
}
