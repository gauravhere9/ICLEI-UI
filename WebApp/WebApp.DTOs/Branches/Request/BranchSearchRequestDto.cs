using WebApp.Global.Shared;

namespace WebApp.DTOs.Branches.Request
{
    public class BranchSearchRequestDto : IPaginationRequestDto
    {
        public string BranchCode { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public int CompanyId { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public string OrderBy { get; set; } = "Id";
        public string OrderByDirection { get; set; } = "Desc";
    }
}
