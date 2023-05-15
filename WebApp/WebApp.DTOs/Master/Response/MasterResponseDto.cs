using WebApp.Global.Shared.DTOs;

namespace WebApp.DTOs.Auth.Response
{
    public class MasterResponseDto
    {
        public MasterResponseDto()
        {
            this.UserTypes = new List<DropdownDto>();
            this.SalutationTypes = new List<DropdownDto>();
            this.ProjectTypes = new List<DropdownDto>();
            this.RateTypes = new List<DropdownDto>();
            this.HolidayTypes = new List<DropdownDto>();
            this.BloodGroups = new List<DropdownDto>();
            this.AttendanceStatus = new List<DropdownDto>();
        }

        public IList<DropdownDto>? UserTypes { get; set; } = null;
        public IList<DropdownDto>? SalutationTypes { get; set; } = null;
        public IList<DropdownDto>? ProjectTypes { get; set; } = null;
        public IList<DropdownDto>? RateTypes { get; set; } = null;
        public IList<DropdownDto>? HolidayTypes { get; set; } = null;
        public IList<DropdownDto>? BloodGroups { get; set; } = null;
        public IList<DropdownDto>? AttendanceStatus { get; set; } = null;
    }
}
