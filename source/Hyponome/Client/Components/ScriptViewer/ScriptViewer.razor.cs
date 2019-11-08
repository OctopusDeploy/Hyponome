using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Hyponome.Client.Components.PullRequests;
using Hyponome.Client.Utils;
using Hyponome.Shared.Models.Response;
using Microsoft.AspNetCore.Components;

namespace Hyponome.Client.Components.ScriptViewer
{
    public class ScriptViewerBase : ComponentBase
    {
        [Inject] public HttpClient HttpClient { get; set; }
        
        [CascadingParameter] public PullRequestFilesBase Parent { get; set; }
        
        [Parameter] public IList<string> Scripts { get; set; }
        [Parameter] public string ScriptMode { get; set; }
        [Parameter] public PullRequestFileResource File { get; set; }

        protected override bool ShouldRender() => File is object || Scripts is object && Scripts.Any();

        protected override void OnInitialized()
        {
            if (Scripts is null)
            {
                Scripts = ScriptHelper.GetScripts(File.Patch);
            }

            if (string.IsNullOrEmpty(ScriptMode))
            {
                ScriptMode = ScriptHelper.GetScriptMode(File.Patch);
            }
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                if (ScriptMode.Equals("powershell", StringComparison.OrdinalIgnoreCase) && Scripts is object && Scripts.Any())
                {
                    var result = await HttpClient.PostJsonAsync<ScriptAnalyzerResult>("api/analyze", Scripts);
                    Parent.ScriptValidationCompleted(File.Sha, result.Errors);
                }
            }

        }
    }
}