namespace WebApp.DTOs.Holiday.Response
{
    public class HolidayResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime HolidayDate { get; set; }
        public int BranchId { get; set; }
        public int HolidayTypeId { get; set; }
        public int? CreatedBy { get; set; }
        public string CreatedByName { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public string? ModifiedByName { get; set; } = string.Empty;
        public DateTime? ModifiedDate { get; set; }
        public bool IsActive { get; set; }
        public DateTime? DeactivatedDate { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DeletedDate { get; set; }
        public string Status { get; set; } = string.Empty;
    }
}
