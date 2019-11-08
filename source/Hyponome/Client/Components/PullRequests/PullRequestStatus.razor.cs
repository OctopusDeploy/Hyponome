using Hyponome.Shared.Models.Response;
using Microsoft.AspNetCore.Components;

namespace Hyponome.Client.Components.PullRequests
{
    public class PullRequestStatusBase : ComponentBase
    {
        [Parameter] public CombinedCommitStatusResource Status { get; set; }

        protected override bool ShouldRender() => Status is object;

        protected string GetStatusColorClass(string state)
        {
            switch (state)
            {
                case "success":
                    return "success";
                case "failure":
                    return "danger";
                default:
                    return "warning";
            }
        }

        protected string GetStatusIcon(string state)
        {
            switch (state)
            {
                case "success":
                    return "check";
                case "failure":
                    return "times";
                default:
                    return "circle";
            }
        }
    }
}