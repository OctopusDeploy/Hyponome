using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Hyponome.Client.Interop;
using Hyponome.Client.Utils;
using Hyponome.Shared.Models.Response;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Hyponome.Client.Components.ScriptModal
{
    public class ScriptModalBase : ComponentBase
    {

        [Inject] private IJSRuntime JSRuntime { get; set; }
        [Inject] private HttpClient HttpClient { get; set; }
        [Parameter] public PullRequestFileResource File { get; set; }
        
        public IList<string> Scripts { get; set; }
        public string ScriptMode { get; set; }
        public bool ScriptValidationInProgress { get; set; } = true;
        public bool ScriptValidationSucceeded => ScriptErrors == null || !ScriptErrors.Any();
        public IEnumerable<ScriptAnalyzerError> ScriptErrors { get; set; }

        protected override bool ShouldRender() =>
            Scripts is object && Scripts.Any();

        protected override void OnInitialized()
        {
            Scripts = ScriptHelper.GetScripts(File.Patch);
            ScriptMode = ScriptHelper.GetScriptMode(File.Patch);
        }
        
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await new HyponomeInterop(JSRuntime).Bootstrap.Tooltip();
            if (firstRender)
            {
                if (ScriptMode.Equals("powershell", StringComparison.OrdinalIgnoreCase) && Scripts is object && Scripts.Any())
                {
                    var result = await HttpClient.PostJsonAsync<ScriptAnalyzerResult>("api/analyze", Scripts);
                    ScriptErrors = result.Errors;
                    ScriptValidationInProgress = false;

                    StateHasChanged();
                }
            }
        }
    }
}