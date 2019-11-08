using System.Collections.Generic;
using System.Linq;
using Hyponome.Shared.Models.Response;
using Microsoft.AspNetCore.Components;

namespace Hyponome.Client.Components.PullRequests
{
    public class PullRequestLabelsBase : ComponentBase
    {
        [Parameter] public IReadOnlyList<LabelResource> Labels { get; set; }

        protected override bool ShouldRender() => Labels is object && Labels.Any();
    }
}