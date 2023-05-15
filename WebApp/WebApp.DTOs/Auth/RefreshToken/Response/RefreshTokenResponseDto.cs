namespace WebApp.DTOs.Auth.RefreshToken.Response
{
    public class RefreshTokenResponseDto
    {
        public long Id { get; set; }
        public string RefreshToken { get; set; } = string.Empty;
        public DateTime? ExpiringDate { get; set; }
        public int UserId { get; set; }
    }
}
