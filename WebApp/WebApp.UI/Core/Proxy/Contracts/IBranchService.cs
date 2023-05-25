using WebApp.DTOs.Branches.Request;
using WebApp.DTOs.Branches.Response;
using WebApp.Global.Response;
using WebApp.Global.Shared;
using WebApp.Global.Shared.DTOs;

namespace WebApp.UI.Core.Proxy.Contracts
{
    public interface IBranchService
    {
        Task<ApiResponse<object>> AddBranchAsync(AddBranchRequestDto requestDto);
        Task<ApiResponse<object>> UpdateBranchAsync(UpdateBranchRequestDto requestDto);
        Task<ApiResponse<object>> UpdateBranchStatusAsync(int id);
        Task<ApiResponse<object>> DeleteBranchAsync(int id);
        Task<ApiResponse<PagedResponseDto<BranchResponseDto>>> GetBranchesWithPSS(BranchSearchRequestDto requestDto);
        Task<ApiResponse<BranchResponseDto>> GetBranchDetailsAsync(int id);
        Task<ApiResponse<DropdownDto>> GetBranchDropdownAsync(int companyId);
    }
}
