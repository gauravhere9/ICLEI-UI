using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using WebApp.Global.Regex;

namespace WebApp.DTOs.Auth.Password.Request
{
    public class ChangePasswordRequestDto
    {
        [Required(ErrorMessage = "New password is required.")]
        [RegularExpression(RegularExpressions.Password, ErrorMessage = "Password should contain 8-16 character, an upper case, a lower case, a number and a special charaterer. For exp. Test@1234.")]
        [DefaultValue("")]
        public string NewPassword { get; set; } = string.Empty;

        [Required(ErrorMessage = "Confirm password is required.")]
        [RegularExpression(RegularExpressions.Password, ErrorMessage = "Password should contain 8-16 character, an upper case, a lower case, a number and a special charaterer. For exp. Test@1234.")]
        [DefaultValue("")]
        public string ConfirmNewPassword { get; set; } = string.Empty;

        [Required(ErrorMessage = "User id is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Please enter valid user id.")]
        [DefaultValue(0)]
        public int UserId { get; set; }
    }
}
