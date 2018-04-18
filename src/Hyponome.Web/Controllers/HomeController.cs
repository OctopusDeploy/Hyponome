using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Hyponome.Web.Controllers
{
    [Route("")]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            Console.WriteLine($"Redirecting to {nameof(PullRequestsController)}");
            return RedirectToAction("Index", "PullRequests");
        }
    }
}
