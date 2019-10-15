using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hyponome.Server.Services
{
    public class GitHubOptions
    {
        public string WebUri { get; set; } = "https://github.com/";
        public string ApiUri { get; set; } = "https://api.github.com/";
        public string OAuthApp { get; set; }
        public string OrganizationName { get; set; }
        public string RepositoryName { get; set; }
        public string OAuthClientId { get; set; }
        public string OAuthClientSecret { get; set; }
    }
}
