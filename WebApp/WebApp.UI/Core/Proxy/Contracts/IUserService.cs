using WebApp.DTOs.User.Request;
using WebApp.DTOs.User.Response;
using WebApp.Global.Response;
using WebApp.Global.Shared;
using WebApp.Global.Shared.DTOs;

namespace WebApp.UI.Core.Proxy.Contracts
{
    public interface IUserService
    {
        Task<ApiResponse<object>> AddUserAsync(AddSiteUserRequestDto requestDto);
        Task<ApiResponse<object>> UpdateUserAsync(UpdateSiteUserRequestDto requestDto);
        Task<ApiResponse<object>> UpdateUserStatusAsync(int id);
        Task<ApiResponse<object>> DeleteUserAsync(int id);
        Task<ApiResponse<PagedResponseDto<SiteUserResponseDto>>> GetUserWithPSS(SearchSiteUserRequestDto requestDto);
        Task<ApiResponse<SiteUserResponseDto>> GetUserDetailsAsync(int id);
        Task<ApiResponse<IList<DropdownDto>>> GetUserDropdownAsync(int branchId);
    }
}
