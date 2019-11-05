using Newtonsoft.Json;
using Octokit;

namespace Hyponome.Shared.Models.Response
{
    public class UserResource
    {
        [JsonConstructor]
        public UserResource() {}

        public UserResource(string login, string htmlUrl)
        {
            Login = login;
            HtmlUrl = htmlUrl;
        }

        public string Login { get; set; }
        public string HtmlUrl { get; set; }

        public static implicit operator UserResource(User user) => new UserResource(user?.Login, user?.HtmlUrl);
    }
}
