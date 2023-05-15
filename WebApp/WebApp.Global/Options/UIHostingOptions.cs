namespace WebApp.Global.Options
{
    public class UIHostingOptions
    {
        public string UIBasePath { get; set; } = default!;
        public string ChangePasswordPath { get; set; } = default!;
        public int ExpirationMinutes { get; set; } = default!;
    }
}
