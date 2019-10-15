using System.Threading.Tasks;
using Microsoft.JSInterop;

namespace Hyponome.Client.Interop
{
    public class HyponomeInterop
    {
        public HyponomeInterop(IJSRuntime jsRuntime)
        {
            Moment = new Moment(jsRuntime);
        }

        public Moment Moment { get; }
        public AceEditor AceEditor { get; }
    }

    
}
