using System.Collections.Generic;
using System.Threading.Tasks;
using Hyponome.Client.Interop;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Hyponome.Client.Components.Ace
{
    public class AceDiffBase : ComponentBase
    {
        [Inject] private IJSRuntime JSRuntime { get; set; }
        
        [Parameter] public IList<string> Scripts { get; set; }
        [Parameter] public string Mode { get; set; }

        protected ElementReference diffElement;

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await new HyponomeInterop(JSRuntime).Ace.Diff(diffElement, Scripts[0], Scripts[1], Mode);
            }
        }
    }
}