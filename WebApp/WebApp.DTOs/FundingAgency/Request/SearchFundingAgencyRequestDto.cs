using WebApp.Global.Shared;

namespace WebApp.DTOs.FundingAgency.Request
{
    public class SearchFundingAgencyRequestDto : IPaginationRequestDto
    {
        public string Name { get; set; } = string.Empty;
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public string OrderBy { get; set; } = "Id";
        public string OrderByDirection { get; set; } = "Desc";
    }
}
