using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace ServiceLayer.Logger
{
    public class HttpRequestLog
    {
        private const int MaxKeepLogMinutes = 10;
        private const int MaxLogsPerTraceId = 50;
        private const int MaxLogsPerTraceIdTrimBy = 10;
        public string TraceIdentifier { get; }
        public DateTime LastAccessed { get; private set; }

        private static readonly ConcurrentDictionary<string, HttpRequestLog> AllHtppRequestLogs =
            new ConcurrentDictionary<string, HttpRequestLog>();

        private readonly List<LogParts> _requestLogs;

        public ImmutableList<LogParts> RequestLogs => _requestLogs.ToImmutableList();

        public HttpRequestLog(string traceIdentifier)
        {
            TraceIdentifier = traceIdentifier;
            LastAccessed = DateTime.UtcNow;
            _requestLogs = new List<LogParts>();

            ClearOldLogs(MaxKeepLogMinutes);

        }
        public static void AddLog(string traceIdentifier, LogLevel logLevel, EventId eventId, string eventString)
        {
            var thisSessionLog = AllHtppRequestLogs.GetOrAdd(traceIdentifier,
                x => new HttpRequestLog(traceIdentifier));

            if (thisSessionLog._requestLogs.Count > MaxLogsPerTraceId)
            {
                thisSessionLog._requestLogs.RemoveRange(0, MaxLogsPerTraceIdTrimBy);
            }

            thisSessionLog._requestLogs.Add(new LogParts(logLevel, eventId, eventString));
            thisSessionLog.LastAccessed = DateTime.UtcNow;
        }
        private void ClearOldLogs(int maxKeepLogMinutes)
        {
            var logsToRemove =
                AllHtppRequestLogs.Values.OrderBy(x => x.LastAccessed)
                .Where(x => DateTime.UtcNow.Subtract(x.LastAccessed).TotalMinutes > MaxKeepLogMinutes);
            RemoveLogs(logsToRemove);
        }

        private void RemoveLogs(IEnumerable<HttpRequestLog> logsToRemove)
        {
            foreach (var logToRemove in logsToRemove)
            {
                HttpRequestLog value;
                AllHtppRequestLogs.TryRemove(logToRemove.TraceIdentifier, out value);
            }
        }

        public override string ToString()
        {
            return $"At time: {LastAccessed:s}, Logs : {string.Join("/n", _requestLogs.Select(x => x.ToString()))}";
        }

        internal static HttpRequestLog GetHttpRequestLog(string traceIdentifier)
        {
            HttpRequestLog result;
            if (AllHtppRequestLogs.TryGetValue(traceIdentifier, out result))
            {
                return result;
            }

            result = new HttpRequestLog(traceIdentifier);
            var oldest = AllHtppRequestLogs.Values.OrderBy(x => x.LastAccessed).FirstOrDefault();

            result._requestLogs.Add(new LogParts(LogLevel.Warning, new EventId(1, "EfCoreInAction"),
                $"Could not find the log you asked for. I have {AllHtppRequestLogs.Keys.Count} logs" +
                (oldest == null ? "." : $" the oldest is {oldest.LastAccessed:s}")));

            return result;
        }
    }

}
