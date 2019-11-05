using System.Collections.Generic;
using Hyponome.Shared.Models.Response;
using Microsoft.AspNetCore.Components;

namespace Hyponome.Client.Components.ScriptViewer
{
    public class ScriptViewerBase : ComponentBase
    {
        [Parameter] public IList<string> Scripts { get; set; }
        [Parameter] public string ScriptMode { get; set; }
        [Parameter] public PullRequestFileResource File { get; set; }
    }
}