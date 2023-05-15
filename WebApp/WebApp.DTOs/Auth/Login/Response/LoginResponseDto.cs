namespace WebApp.DTOs.Auth.Login.Response
{
    public class LoginResponseDto
    {
        public string Token { get; set; } = string.Empty;
        public int ExpiringIn { get; set; } = 0;
        public string RefreshToken { get; set; } = string.Empty;
        public int UserId { get; set; } = 0;
    }
}
