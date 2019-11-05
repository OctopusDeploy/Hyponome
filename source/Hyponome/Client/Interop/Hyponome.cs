using System.Threading.Tasks;
using Microsoft.JSInterop;

namespace Hyponome.Client.Interop
{
    public class HyponomeInterop
    {
        public HyponomeInterop(IJSRuntime jsRuntime)
        {
            Ace = new Ace(jsRuntime);
            Bootstrap = new Bootstrap(jsRuntime);
            Moment = new Moment(jsRuntime);
        }

        public Ace Ace { get; }
        public Bootstrap Bootstrap { get; }
        public Moment Moment { get; }
    }
}
