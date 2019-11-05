using System.Collections.Generic;
using Hyponome.Shared.Models.Response;
using Microsoft.AspNetCore.Components;

namespace Hyponome.Client.Components.PullRequests
{
    public class PullRequestListBase : ComponentBase
    {
        [Parameter] public IEnumerable<IssueResource> PullRequests { get; set; }
    }
}
