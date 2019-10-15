using Microsoft.JSInterop;

namespace Hyponome.Client.Interop
{
    public class AceEditor
    {
        private readonly JSRuntime jsRuntime;

        public AceEditor(JSRuntime jsRuntime)
        {
            this.jsRuntime = jsRuntime;
        }
    }
}
