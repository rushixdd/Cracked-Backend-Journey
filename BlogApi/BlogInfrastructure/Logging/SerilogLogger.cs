using Serilog;

namespace BlogInfrastructure.Logging
{
    public static class SerilogLogger
    {
        public static void Configure()
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .WriteTo.File("Logs/log-.txt", rollingInterval: RollingInterval.Day)
                .Enrich.FromLogContext()
                .CreateLogger();
        }
    }
}
