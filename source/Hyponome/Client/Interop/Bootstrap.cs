using System.Threading.Tasks;
using Microsoft.JSInterop;

namespace Hyponome.Client.Interop
{
    public class Bootstrap
    {
        private readonly IJSRuntime jsRuntime;

        public Bootstrap(IJSRuntime jsRuntime)
        {
            this.jsRuntime = jsRuntime;
        }

        public async ValueTask Tooltip()
        {
            await jsRuntime.InvokeVoidAsync("hyponome.bootstrap.tooltip");
        }
    }
}