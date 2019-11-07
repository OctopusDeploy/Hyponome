using System.Collections.Generic;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Hyponome.Client.Utils
{
    public static class ScriptHelper
    {
        static readonly Regex syntaxRegex = new Regex("^.*(?:Octopus.Action.Script.Syntax)[\\W]*(?<syntax>\\w*).*$", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Multiline);
        static readonly Regex scriptBodyRegex = new Regex("^(?:[-+])(?<script>.*Octopus.Action.Script.ScriptBody.*)$", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Multiline);

        public static bool HasScript(string patch) => !string.IsNullOrEmpty(patch) && scriptBodyRegex.IsMatch(patch);
        public static string GetScriptMode(string patch) =>
            !string.IsNullOrEmpty(patch) && syntaxRegex.IsMatch(patch) ? syntaxRegex.Match(patch).Groups["syntax"].Value.ToLower() : "powershell";

        public static IList<string> GetScripts(string patch)
        {
            if (string.IsNullOrEmpty(patch) || !scriptBodyRegex.IsMatch(patch)) return null;
            
            var scripts = new List<string>();
            var matches = scriptBodyRegex.Matches(patch);
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