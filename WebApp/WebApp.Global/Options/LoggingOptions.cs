namespace WebApp.Global.Options
{
    public class LoggingOptions
    {
        public bool UseExtendedIntegrationLogging { get; set; } = default!;
        public LogLevelOptions LogLevel { get; set; } = default!;
    }
}
