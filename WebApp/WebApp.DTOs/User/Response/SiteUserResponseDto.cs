namespace WebApp.DTOs.User.Response
{
    public class SiteUserResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string EmployeeCode { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Mobile { get; set; } = string.Empty;
        public DateTime? DOB { get; set; }
        public int GenderId { get; set; }
        public string GenderName { get; set; } = string.Empty;
        public int? BloodGroupId { get; set; }
        public string? BloodGroupName { get; set; }
        public int? MaritalStatusId { get; set; }
        public string? MaritalStatusName { get; set; }
        public string? SpouseName { get; set; } = string.Empty;
        public int? BranchId { get; set; }
        public string? BranchName { get; set; }
        public int? DesignationId { get; set; }
        public string? DesignationName { get; set; }
        public int? ReportingTo { get; set; }
        public string? ReportingToName { get; set; }
        public string? PAN { get; set; } = string.Empty;
        public string? AadharNo { get; set; } = string.Empty;
        public string? EmergencyPerson { get; set; } = string.Empty;
        public string? EmergencyContact { get; set; } = string.Empty;
        public int UserTypeId { get; set; }
        public string UserTypeName { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string PasswordSalt { get; set; } = string.Empty;

        public int? CreatedBy { get; set; }
        public string? CreatedByName { get; set; }
        public DateTime CreatedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public string? ModifiedByName { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public bool IsActive { get; set; }
        public DateTime? DeactivatedDate { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DeletedDate { get; set; }
        public string? Status { get; set; }
    }
}
