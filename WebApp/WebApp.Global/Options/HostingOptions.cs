namespace WebApp.Global.Options
{
    public class HostingOptions
    {
        public bool UseHttps { get; set; } = default!;
        public int HttpsPort { get; set; } = default!;
        public string BasePath { get; set; } = default!;
        public UIHostingOptions UIHosting { get; set; } = default!;
    }
}
