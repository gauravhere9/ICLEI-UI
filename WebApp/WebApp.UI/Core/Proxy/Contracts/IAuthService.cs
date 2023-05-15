using WebApp.DTOs.Auth.Login.Request;
using WebApp.DTOs.Auth.Password.Request;
using WebApp.DTOs.Auth.RefreshToken.Request;
using WebApp.Global.Response;

namespace WebApp.UI.Core.Proxy.Contracts
{
    public interface IAuthService
    {
        Task<ApiResponse> Login(LoginRequestDto requestDto);
        Task<ApiResponse> Logout(RefreshTokenRequestDto requestDto);
        Task<ApiResponse> RefreshToken(RefreshTokenRequestDto requestDto);
        Task<ApiResponse> ForgotPassword(AddForgotPasswordRequestDto requestDto);
        Task<ApiResponse> ResetPassword(ResetPasswordRequestDto requestDto);
        Task<ApiResponse> ChangePassword(ChangePasswordRequestDto requestDto);
    }
}
