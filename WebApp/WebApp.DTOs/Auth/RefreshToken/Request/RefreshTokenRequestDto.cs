using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using WebApp.Global.Attributes;

namespace WebApp.DTOs.Auth.RefreshToken.Request
{
    public class RefreshTokenRequestDto
    {
        [Required(ErrorMessage = "User id is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Please enter valid user id.")]
        [DefaultValue(0)]
        public int UserId { get; set; }

        [Required(ErrorMessage = "Refresh token is required.")]
        [NoConsecutiveSpace]
        [NoSpaceChar]
        public string RefreshToken { get; set; } = string.Empty;
    }
}
