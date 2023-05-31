using WebApp.DTOs.Designation.Request;
using WebApp.DTOs.Designation.Response;
using WebApp.Global.Response;
using WebApp.Global.Shared;
using WebApp.Global.Shared.DTOs;

namespace WebApp.UI.Core.Proxy.Contracts
{
    public interface IDesignationService
    {
        Task<ApiResponse<object>> AddDesignationAsync(AddDesignationRequestDto requestDto);
        Task<ApiResponse<object>> UpdateDesignationAsync(UpdateDesignationRequestDto requestDto);
        Task<ApiResponse<object>> UpdateDesignationStatusAsync(int id);
        Task<ApiResponse<object>> DeleteDesignationAsync(int id);
        Task<ApiResponse<PagedResponseDto<DesignationResponseDto>>> GetDesignationesWithPSS(DesignationSearchRequestDto requestDto);
        Task<ApiResponse<DesignationResponseDto>> GetDesignationDetailsAsync(int id);
        Task<ApiResponse<IList<DropdownDto>>> GetDesignationDropdownAsync();
    }
}
