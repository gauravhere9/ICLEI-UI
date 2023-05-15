namespace WebApp.Global.Options
{
    public class SMTPOptions
    {
        public string ServerName { get; set; } = default!;
        public string ServerPort { get; set; } = default!;
        public string SmtpUsername { get; set; } = default!;
        public string SmtpPassword { get; set; } = default!;
        public string FromEmail { get; set; } = default!;
        public string FromName { get; set; } = default!;
    }
}
