using System.Net.Http;
using System.Threading.Tasks;
using Hyponome.Shared.Models.Response;
using Microsoft.AspNetCore.Components;

namespace Hyponome.Client.Pages
{
    public class PullRequestBase : ComponentBase
    {
        [Parameter]
        public int Number { get; set; }
        [Inject] private HttpClient Http { get; set; }

        public PullRequestResource PullRequest;

        protected override async Task OnInitializedAsync() =>
            PullRequest = await Http.GetJsonAsync<PullRequestResource>($"api/pulls/{Number}");
    }
}
