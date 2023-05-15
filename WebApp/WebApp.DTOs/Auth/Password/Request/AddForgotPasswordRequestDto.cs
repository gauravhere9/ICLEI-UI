using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using WebApp.Global.Attributes;
using WebApp.Global.Regex;

namespace WebApp.DTOs.Auth.Password.Request
{
    public class AddForgotPasswordRequestDto
    {
        [Required(ErrorMessage = "Email is required.")]
        [NoConsecutiveSpace]
        [NoSpaceChar]
        [RegularExpression(RegularExpressions.Email, ErrorMessage = "Email is not in correct format.")]
        [DefaultValue("")]
        public string Email { get; set; } = string.Empty;
    }
}
