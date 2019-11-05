using System.Threading.Tasks;
using Microsoft.JSInterop;

namespace Hyponome.Client.Interop
{
    public class Moment
    {
        private readonly IJSRuntime jsRuntime;
        public Moment(IJSRuntime jsRuntime)
        {
            this.jsRuntime = jsRuntime;
        }

        public ValueTask<string> FromNow(string dateTime)
        {
            return jsRuntime.InvokeAsync<string>("hyponome.moment.fromNow", dateTime);
        }
    }
}