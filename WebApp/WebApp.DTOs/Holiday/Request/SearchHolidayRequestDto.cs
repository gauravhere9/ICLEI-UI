using WebApp.Global.Shared;

namespace WebApp.DTOs.Holiday.Request
{
    public class SearchHolidayRequestDto : IPaginationRequestDto
    {
        public string? Name { get; set; } = string.Empty;
        public int? BranchId { get; set; }
        public int? HolidayTypeId { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public string OrderBy { get; set; } = "Id";
        public string OrderByDirection { get; set; } = "Desc";
    }
}
