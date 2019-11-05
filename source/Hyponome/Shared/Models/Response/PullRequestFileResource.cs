using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Octokit;

namespace Hyponome.Shared.Models.Response
{
    public class PullRequestFileResource
    {
        [JsonConstructor]
        public PullRequestFileResource() { }

        public PullRequestFileResource(string fileName, string sha, string patch)
        {
            FileName = fileName;
            Sha = sha;
            Patch = patch;
        }

        public string FileName { get; set; }
        public string Sha { get; set; }
        public string Patch { get; set; }

//        public static implicit operator PullRequestFileResource(PullRequestFile file) =>
//            new PullRequestFileResource(file.FileName, file.Sha, file.Patch);
    }
}
