namespace WebApp.Global.Options
{
    public class AuthenticationOptions
    {
        public bool Enabled { get; set; } = default!;
        public JWTTokenOptions JWTToken { get; set; } = default!;
    }
}
