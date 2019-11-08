using Hyponome.Server.Services;
using Hyponome.Server.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Hyponome.Server.Controllers
{
    [Route("api")]
    public class RootController : ControllerBase
    {
        private readonly GitHubOptions gitHubOptions;

        public RootController(IOptions<GitHubOptions> gitHubOptionsAccessor)
        {
            gitHubOptions = gitHubOptionsAccessor.Value;
        }
        
        public IActionResult Index()
        {
            return Ok(new
            {
                Application = gitHubOptions.OAuthApp,
                Version = typeof(RootController).Assembly.GetInformationalVersion(),
            });
        }
    }
}