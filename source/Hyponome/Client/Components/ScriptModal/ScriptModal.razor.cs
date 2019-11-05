using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Hyponome.Client.Interop;
using Hyponome.Shared.Models.Response;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Hyponome.Client.Components.ScriptModal
{
    public class ScriptModalBase : ComponentBase
    {
        static readonly Regex syntaxRegex = new Regex("^.*(?:Octopus.Action.Script.Syntax)[\\W]*(?<syntax>\\w*).*$", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Multiline);
        static readonly Regex scriptBodyRegex = new Regex("^(?:[-+])(?<script>.*Octopus.Action.Script.ScriptBody.*)$", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Multiline);

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
            Scripts = GetScripts();
            ScriptMode = GetScriptMode();
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

        private string GetScriptMode() =>
            syntaxRegex.IsMatch(File.Patch) ? syntaxRegex.Match(File.Patch).Groups["syntax"].Value.ToLower() : "powershell";

        private IList<string> GetScripts()
        {
            if (!scriptBodyRegex.IsMatch(File.Patch)) return null;
            
            var scripts = new List<string>();
            var matches = scriptBodyRegex.Matches(File.Patch);
            for (var i = 0; i < matches.Count; i++)
            {
                var match = matches[i];
                var scriptBody = (string)JsonConvert.DeserializeObject<JObject>($"{{{match.Groups["script"].Value}}}")["Octopus.Action.Script.ScriptBody"];
                scripts.Add(scriptBody);
            }

            return scripts;
        }
    }
}