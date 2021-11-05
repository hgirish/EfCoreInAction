﻿using Microsoft.AspNetCore.Mvc;
using ServiceLayer.Logger;

namespace BookApp.Controllers
{
    public class BaseTraceController: Controller
    {
        protected void SetupTraceInfo()
        {
            ViewData["TraceIdent"] = HttpContext.TraceIdentifier;
            ViewData["NumLogs"] = HttpRequestLog.GetHttpRequestLog(HttpContext.TraceIdentifier).RequestLogs.Count;
        }
    }
}
