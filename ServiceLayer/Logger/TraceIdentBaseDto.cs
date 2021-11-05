using System.Linq;

namespace ServiceLayer.Logger
{
    public class TraceIdentBaseDto
    {
        public string TraceIdentifier { get; private set; }
        public int NumLogs { get; set; }
        public TraceIdentBaseDto(string traceIdentifier)
        {
            TraceIdentifier = traceIdentifier;
            NumLogs = HttpRequestLog.GetHttpRequestLog(traceIdentifier)
                .RequestLogs.Count;
        }
    }

}
