using WebApp.Global.Shared;

namespace WebApp.DTOs.User.Request
{
    public class SearchSiteUserRequestDto : IPaginationRequestDto
    {
        public string Name { get; set; } = string.Empty;
        public string EmployeeCode { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Mobile { get; set; } = string.Empty;
        public int BranchId { get; set; }
        public int DesignationId { get; set; }
        public int UserTypeId { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public string OrderBy { get; set; } = "Id";
        public string OrderByDirection { get; set; } = "Desc";
    }
}
