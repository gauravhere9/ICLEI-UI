using WebApp.DTOs.Auth.Login.Request;
using WebApp.DTOs.Auth.Login.Response;
using WebApp.DTOs.Auth.Password.Request;
using WebApp.DTOs.Auth.RefreshToken.Request;
using WebApp.DTOs.Auth.RefreshToken.Response;
using WebApp.Global.Response;

namespace WebApp.UI.Core.Proxy.Contracts
{
    public interface IAuthService
    {
        Task<ApiResponse<LoginResponseDto>> LoginAsync(LoginRequestDto requestDto);
        Task<ApiResponse<object>> LogoutAsync(RefreshTokenRequestDto requestDto);
        Task<ApiResponse<RefreshTokenResponseDto>> RefreshTokenAsync(RefreshTokenRequestDto requestDto);
        Task<ApiResponse<bool>> ForgotPasswordAsync(AddForgotPasswordRequestDto requestDto);
        Task<ApiResponse<bool>> ResetPasswordAsync(ResetPasswordRequestDto requestDto);
        Task<ApiResponse<bool>> ChangePasswordAsync(ChangePasswordRequestDto requestDto);
    }
}
