using WebApp.DTOs.Auth.Response;
using WebApp.Global.Response;

namespace WebApp.UI.Core.Proxy.Contracts
{
    public interface IMasterService
    {
        Task<ApiResponse<MasterResponseDto>> GetMasterDropdown();
    }
}
