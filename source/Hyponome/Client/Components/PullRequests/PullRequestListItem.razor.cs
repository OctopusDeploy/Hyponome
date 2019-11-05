using System;
using Hyponome.Shared.Models.Response;
using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;

namespace Hyponome.Client.Components.PullRequests
{
    public class PullRequestListItemBase : ComponentBase
    {
        [Parameter] public IssueResource PullRequest { get; set; }

        protected override void OnParametersSet()
        {
            Console.WriteLine(JsonConvert.SerializeObject(PullRequest));
        }
    }
}