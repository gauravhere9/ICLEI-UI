namespace WebApp.Global.Options
{
    public class ApplicationOptions
    {
        public string AppName { get; set; } = string.Empty;
        public CacheOptions Cache { get; set; } = default!;
        public LoggingOptions Logging { get; set; } = default!;
        public HostingOptions Hosting { get; set; } = default!;
        public SwaggerOptions Swagger { get; set; } = default!;
        public SMTPOptions SMTP { get; set; } = default!;
    }
}
