using WebApp.DTOs.CompanyInfo.Request;
using WebApp.DTOs.CompanyInfo.Response;
using WebApp.Global.Response;
using WebApp.Global.Shared.DTOs;

namespace WebApp.UI.Core.Proxy.Contracts
{
    public interface ICompanyService
    {
        Task<ApiResponse<CompanyInfoResponseDto>> GetCompanyDetailsAsync();
        Task<ApiResponse<DropdownDto>> GetCompanyDropdownAsync();
        Task<ApiResponse<object>> AddOrUpdateCompanyAsync(AddorUpdateCompanyInfoRequestDto requestDto);
    }
}
