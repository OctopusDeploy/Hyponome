using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Hyponome.Client.Interop;
using Hyponome.Shared.Models.Response;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Hyponome.Client.Components.PullRequests
{
    public class PullRequestDetailsBase : ComponentBase
    {
        [Inject] public IJSRuntime JSRuntime { get; set; }
        
        [Parameter] public PullRequestResource PullRequest { get; set; }

        protected bool ScriptValidationInProgress { get; set; } = true;
        protected IDictionary<string, IEnumerable<ScriptAnalyzerError>> ScriptErrors { get; } = new Dictionary<string, IEnumerable<ScriptAnalyzerError>>();
        protected bool ShowScript { get; set; } = true;

        protected IEnumerable<ScriptAnalyzerError> GetScriptErrorsForFile(string sha) =>
            ScriptErrors.TryGetValue(sha, out var errors) ? errors : null;
        
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await new HyponomeInterop(JSRuntime).Bootstrap.Tooltip();
        }

        public void ScriptValidationCompleted(string fileSha, IEnumerable<ScriptAnalyzerError> errors)
        {
            if (ScriptErrors.ContainsKey(fileSha))
            {
                ScriptErrors[fileSha] = errors;
            }
            else
            {
                ScriptErrors.Add(fileSha, errors);
            }

            ScriptValidationInProgress = false;
            StateHasChanged();
        }
    }
}
