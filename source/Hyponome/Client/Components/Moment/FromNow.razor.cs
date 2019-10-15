using System;
using System.Threading.Tasks;
using Hyponome.Client.Interop;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Hyponome.Client.Components.Moment
{
    public class FromNowBase : ComponentBase
    {
        [Inject] private IJSRuntime JSRuntime { get; set; }

        [Parameter] public DateTimeOffset Value { get; set; }

        protected string FromNowText { get; set; }
        protected string Title => Value.LocalDateTime.ToString("MMMM d, yyyy, hh:mm tt 'GMT'zz");
    

        protected override async Task OnInitializedAsync() =>
            FromNowText = await new HyponomeInterop(JSRuntime).Moment.FromNow(Value.ToString("yyyy-MM-ddTHH:mm:ssZ"));
    }
}
