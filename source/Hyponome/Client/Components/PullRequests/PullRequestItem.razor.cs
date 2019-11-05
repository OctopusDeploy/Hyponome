using System;
using System.Threading.Tasks;
using Hyponome.Client.Interop;
using Hyponome.Shared.Models.Response;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Hyponome.Client.Components.PullRequests
{
    public class PullRequestItemBase : ComponentBase
    {
        [Inject] public IJSRuntime JSRuntime { get; set; }
        [Parameter] public PullRequestResource PullRequest { get; set; }
    }
}
