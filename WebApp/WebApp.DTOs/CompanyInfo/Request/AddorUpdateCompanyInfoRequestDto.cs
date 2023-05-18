using System.ComponentModel.DataAnnotations;
using WebApp.Global.Attributes;

namespace WebApp.DTOs.CompanyInfo.Request
{
    public class AddorUpdateCompanyInfoRequestDto
    {
        [Required(ErrorMessage = "Company name is required")]
        [NoSpaceChar]
        [NoConsecutiveSpace]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Company address is required")]
        [NoSpaceChar]
        [NoConsecutiveSpace]
        public string Address { get; set; } = string.Empty;

        [Required(ErrorMessage = "Company email is required")]
        [NoSpaceChar]
        [NoConsecutiveSpace]
        [EmailAddress(ErrorMessage = "Please enter a valid email address")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Company phone is required")]
        [NoSpaceChar]
        [NoConsecutiveSpace]
        [Phone(ErrorMessage = "Please enter a valid phone number")]
        public string Phone { get; set; } = string.Empty;
        public string? Fax { get; set; } = string.Empty;
        public string? Website { get; set; } = string.Empty;
    }
}
