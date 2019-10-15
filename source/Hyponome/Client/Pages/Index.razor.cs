﻿using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Hyponome.Shared.Models.Response;
using Microsoft.AspNetCore.Components;

namespace Hyponome.Client.Pages
{
    public class IndexBase : ComponentBase
    {
        public IEnumerable<IssueResource> PullRequests;

        [Inject] private HttpClient Http { get; set; }

        protected override async Task OnInitializedAsync() =>
            PullRequests = await Http.GetJsonAsync<IEnumerable<IssueResource>>("api/pulls");
    }
}
