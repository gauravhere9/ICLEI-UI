using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using WebApp.Global.Attributes;
using static WebApp.Global.Constants.Enumurations;

namespace WebApp.DTOs.User.Request
{
    public class AddSiteUserRequestDto
    {
        [Required(ErrorMessage = "Name is required")]
        [NoSpaceChar]
        [NoConsecutiveSpace]
        [StringLength(500, MinimumLength = 3, ErrorMessage = "Name must contain 3-100 characters")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Employee code is required")]
        [NoSpaceChar]
        [NoConsecutiveSpace]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Employee code must contain 3-10 characters")]
        public string EmployeeCode { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required")]
        [NoSpaceChar]
        [NoConsecutiveSpace]
        [EmailAddress(ErrorMessage = "Please enter a valid email address")]
        public string Email { get; set; } = string.Empty;
        public string? Mobile { get; set; } = string.Empty;
        public DateTime? DOB { get; set; }

        [Required(ErrorMessage = "Gender is required")]
        public GenderTypes GenderId { get; set; }
        public BloodGroupTypes? BloodGroupId { get; set; }
        public MaritalStatus? MaritalStatusId { get; set; }
        public string? SpouseName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Branch is required")]
        [DefaultValue(0)]
        [Range(1, int.MaxValue, ErrorMessage = "Please enter correct branch")]
        public int? BranchId { get; set; }

        [Required(ErrorMessage = "Designation is required")]
        [DefaultValue(0)]
        [Range(1, int.MaxValue, ErrorMessage = "Please enter correct designation")]
        public int? DesignationId { get; set; }
        public int? ReportingTo { get; set; }
        public string? PAN { get; set; } = string.Empty;
        public string? AadharNo { get; set; } = string.Empty;
        public string? EmergencyPerson { get; set; } = string.Empty;
        public string? EmergencyContact { get; set; } = string.Empty;

        [Required(ErrorMessage = "User type is required")]
        public UserTypes UserTypeId { get; set; }
    }
}
