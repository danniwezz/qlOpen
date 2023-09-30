using Serilog;

using Yuniql.Extensibility;

namespace Shared.Yuniql
{
    public class ConsoleTraceService : ITraceService
    {
        public ConsoleTraceService()
        {
        }

        public bool IsDebugEnabled { get; set; } = false;
        public bool IsTraceSensitiveData { get; set; } = false;
        public bool IsTraceToDirectory { get; set; } = false;
        public bool IsTraceToFile { get; set; } = false;
        public string TraceDirectory { get; set; } = "/";

        public void Info(string message, object? payload = null)
        {
            Log.Information(message, payload);
        }

        public void Error(string message, object? payload = null)
        {
            Log.Error(message, payload);
        }

        public void Debug(string message, object? payload = null)
        {
            if (IsDebugEnabled)
            {
                Log.Debug(message, payload);
            }
        }

        public void Success(string message, object? payload = null)
        {
            Log.Information(message, payload);

        }

        public void Warn(string message, object? payload = null)
        {
            Log.Warning(message, payload);
        }
    }
}
