namespace WebApp.Global.Options
{
    public class SwaggerOptions
    {
        public string Title { get; set; } = default!;
        public string[] Versions { get; set; } = default!;
        public string Description { get; set; } = default!;
        public ContactOptions Contact { get; set; } = default!;
    }
}
