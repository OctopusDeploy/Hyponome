using System.Collections.Generic;
using Hyponome.Shared.Models.Response;
using Microsoft.AspNetCore.Components;

namespace Hyponome.Client.Components.ScriptModal
{
    public class ScriptDialogBase : ComponentBase
    {
        [CascadingParameter] public ScriptModalBase Parent { get; set; }
    }
}