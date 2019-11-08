using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Hyponome.Client.Interop;
using Hyponome.Shared.Models.Response;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Hyponome.Client.Components.PullRequests
{
    public class PullRequestDetailsBase : ComponentBase
    {
        [Parameter] public PullRequestResource PullRequest { get; set; }
    }
}
