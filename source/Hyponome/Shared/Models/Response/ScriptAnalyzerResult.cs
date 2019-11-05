using System.Collections.Generic;
using System.Linq;

namespace Hyponome.Shared.Models.Response
{
    public struct ScriptAnalyzerResult
    {
        private ScriptAnalyzerResult(IList<ScriptAnalyzerError> errors)
        {
            Errors = errors;
        }
        
        public IEnumerable<ScriptAnalyzerError> Errors { get; set; }
        
        public static ScriptAnalyzerResult Success() => new ScriptAnalyzerResult();
        public static ScriptAnalyzerResult Failure(IList<ScriptAnalyzerError> errors) => new ScriptAnalyzerResult(errors);
    }

    public struct ScriptAnalyzerError
    {
        public ScriptAnalyzerError(int line, int column, string message)
        {
            Line = line;
            Column = column;
            Message = message;
        }
        
        public int Line { get; set; }
        public int Column { get; set; }
        public string Message { get; set; }
    }
}