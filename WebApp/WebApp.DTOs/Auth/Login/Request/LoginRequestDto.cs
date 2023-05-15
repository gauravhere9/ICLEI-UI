using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using WebApp.Global.Attributes;
using WebApp.Global.Regex;

namespace WebApp.DTOs.Auth.Login.Request
{
    public class LoginRequestDto
    {
        [Required(ErrorMessage = "Username/Email is required.")]
        [NoConsecutiveSpace]
        [NoSpaceChar]
        [RegularExpression(RegularExpressions.Email, ErrorMessage = "Email is not in correct format.")]
        [DefaultValue("")]
        public string Username { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required.")]
        [NoConsecutiveSpace]
        [NoSpaceChar]
        [RegularExpression(RegularExpressions.Password, ErrorMessage = "Password should contain 8-16 character, an upper case, a lower case, a number and a special charaterer. For exp. Test@1234.")]
        [DefaultValue("")]
        public string Password { get; set; } = string.Empty;
    }
}
