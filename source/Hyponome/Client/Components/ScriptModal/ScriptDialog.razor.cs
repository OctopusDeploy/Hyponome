using System.Collections.Generic;
using Hyponome.Shared.Models.Response;
using Microsoft.AspNetCore.Components;

namespace Hyponome.Client.Components.ScriptModal
{
    public class ScriptDialogBase : ComponentBase
    {
        [CascadingParameter] public ScriptModalBase Parent { get; set; }
//        [CascadingParameter] public IList<string> Scripts { get; set; }
//        [CascadingParameter] public string ScriptMode { get; set; }
//        [CascadingParameter] public PullRequestFileResource File { get; set; }
//        [CascadingParameter(Name = "InProgress")] public bool InProgress { get; set; }
//        [CascadingParameter(Name = "Succeeded")] public bool Succeeded { get; set; }
//        [CascadingParameter] public IEnumerable<ScriptAnalyzerError> ScriptErrors { get; set; }
    }
}