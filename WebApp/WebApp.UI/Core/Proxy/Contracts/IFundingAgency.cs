using WebApp.DTOs.FundingAgency.Request;
using WebApp.DTOs.FundingAgency.Response;
using WebApp.Global.Response;
using WebApp.Global.Shared;
using WebApp.Global.Shared.DTOs;

namespace WebApp.UI.Core.Proxy.Contracts
{
    public interface IFundingAgency
    {
        Task<ApiResponse<object>> AddFundingAgencyAsync(AddFundingAgencyRequestDto requestDto);
        Task<ApiResponse<object>> UpdateFundingAgencyAsync(UpdateFundingAgencyRequestDto requestDto);
        Task<ApiResponse<object>> UpdateFundingAgencyStatusAsync(int id);
        Task<ApiResponse<object>> DeleteFundingAgencyAsync(int id);
        Task<ApiResponse<PagedResponseDto<FundingAgencyResponseDto>>> GetFundingAgencyWithPSS(SearchFundingAgencyRequestDto requestDto);
        Task<ApiResponse<FundingAgencyResponseDto>> GetFundingAgencyDetailsAsync(int id);
        Task<ApiResponse<IList<DropdownDto>>> GetFundingAgencyDropdownAsync();
    }
}
