namespace WebApp.Global.Options
{
    public class JWTTokenOptions
    {
        public string Secret { get; set; } = default!;
        public int ExpirationTime { get; set; } = default!;
        public string Issuer { get; set; } = default!;
    }
}
