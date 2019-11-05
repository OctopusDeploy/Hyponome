using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Octokit;

namespace Hyponome.Shared.Models.Response
{
    public class GitReferenceResource
    {
        [JsonConstructor]
        public GitReferenceResource() { }

        public GitReferenceResource(string label)
        {
            Label = label;
        }

        public string Label { get; set; }

        public static implicit operator GitReferenceResource(GitReference @ref) => new GitReferenceResource(@ref?.Label);
    }
}
