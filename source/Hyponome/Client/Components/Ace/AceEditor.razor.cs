using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Hyponome.Client.Interop;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Hyponome.Client.Components.Ace
{
    public class AceEditorBase : ComponentBase
    {
        [Inject] private IJSRuntime JSRuntime { get; set; }
        protected ElementReference aceEditorElement;
        
        [Parameter] public string Key { get; set; }
        [Parameter] public string Script { get; set; }
        [Parameter] public string Mode { get; set; }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await new HyponomeInterop(JSRuntime).Ace.Editor(aceEditorElement, Mode);
            }
        }
    }
}