using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Net;
using System.Threading;
using Hyponome.Shared.Models.Response;
using Microsoft.AspNetCore.Mvc;

namespace Hyponome.Server.Controllers
{
    [Route("api/analyze")]
    [ApiController]
    public class ScriptAnalyzerController : ControllerBase
    {
        [HttpPost]
        public IActionResult Index([FromBody] IEnumerable<string> scripts)
        {
            var scriptErrors = new List<ScriptAnalyzerError>();
            foreach (var script in scripts)
            {
                PSParser.Tokenize(script, out var errors);
                if (!errors.Any()) continue;

                scriptErrors.AddRange(errors.Select(
                    error => new ScriptAnalyzerError(error.Token.StartLine,error.Token.StartColumn,error.Message))
                );
            }
            
            return scriptErrors.Any() 
                ? StatusCode((int)HttpStatusCode.PartialContent, ScriptAnalyzerResult.Failure(scriptErrors)) 
                : Ok(ScriptAnalyzerResult.Success());
        }
    }
}