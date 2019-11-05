using System.Collections.Generic;
using Hyponome.Shared.Models.Response;
using Microsoft.AspNetCore.Components;

namespace Hyponome.Client.Components.ScriptValidation
{
    public class ValidationDetailsBase : ComponentBase
    {
        [Parameter] public IEnumerable<ScriptAnalyzerError> Errors { get; set; }
    }
}