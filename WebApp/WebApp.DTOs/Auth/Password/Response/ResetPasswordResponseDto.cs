namespace WebApp.DTOs.Auth.Password.Response
{
    public class ResetPasswordResponseDto
    {
        public Guid Id { get; set; }
        public int UserId { get; set; }
        public DateTime ExpiringDate { get; set; }
        public bool IsReset { get; set; }
    }
}
