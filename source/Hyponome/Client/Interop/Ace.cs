using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Hyponome.Client.Interop
{
    public class Ace
    {
        private readonly IJSRuntime jsRuntime;

        public Ace(IJSRuntime jsRuntime)
        {
            this.jsRuntime = jsRuntime;
        }

        public async ValueTask Editor(ElementReference element, string mode)
        {
            await jsRuntime.InvokeVoidAsync("hyponome.ace.editor", element, mode);
        }

        public async ValueTask Differ(ElementReference element, string leftScript, string rightScript, string mode)
        {
            await jsRuntime.InvokeVoidAsync("hyponome.ace.differ", element, leftScript, rightScript, mode);
        }
    }
}
