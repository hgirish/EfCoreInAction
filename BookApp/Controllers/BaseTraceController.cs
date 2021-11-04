using Microsoft.AspNetCore.Mvc;

namespace BookApp.Controllers
{
    public class BaseTraceController: Controller
    {
        protected void SetupTraceInfo()
        {
            ViewData["TraceIdent"] = HttpContext.TraceIdentifier;
            // ViewData["NumLogs"] = HttpRequestLog.GetHttpRequestLog(HttpContext.TraceIdentifier).RequestLogs.Count;
        }
    }
}
