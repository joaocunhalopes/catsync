using Serilog;

namespace Util
{
    public static class Log
    {
        // Initialize Serilog logger.
        static Log()
        {
            Serilog.Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Console(outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff} [{Level:u3}] {Message:lj}{NewLine}{Exception}")
                .CreateLogger();
        }

        public static void Information(string messageTemplate, params object[] args)
        {
            Serilog.Log.Information(messageTemplate, args);
        }

        public static void Warning(string messageTemplate, params object[] args)
        {
            Serilog.Log.Warning(messageTemplate, args);
        }

        public static void Error(string messageTemplate, params object[] args)
        {
            Serilog.Log.Error(messageTemplate, args);
        }

        public static void Debug(string messageTemplate, params object[] args)
        {
            Serilog.Log.Debug(messageTemplate, args);
        }
    }
}