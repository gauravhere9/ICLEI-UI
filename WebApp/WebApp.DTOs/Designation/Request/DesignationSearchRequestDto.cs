using WebApp.Global.Shared;

namespace WebApp.DTOs.Designation.Request
{
    public class DesignationSearchRequestDto : IPaginationRequestDto
    {
        public string Name { get; set; } = string.Empty;
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public string OrderBy { get; set; } = "Id";
        public string OrderByDirection { get; set; } = "Desc";
    }
}