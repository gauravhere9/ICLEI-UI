using WebApp.Global.Shared;

namespace WebApp.DTOs.User.Request
{
    public class SearchSiteUserRequestDto : IPaginationRequestDto
    {
        public string SearchTerm { get; set; } = string.Empty;
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public string OrderBy { get; set; } = "Id";
        public string OrderByDirection { get; set; } = "Desc";
    }
}
