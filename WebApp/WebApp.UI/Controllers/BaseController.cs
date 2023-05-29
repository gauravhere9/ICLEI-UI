using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using WebApp.DTOs.Auth.Login.Response;
using WebApp.Global.Extensions;
using WebApp.Global.Options;
using WebApp.Global.Shared;
using WebApp.UI.Core.Proxy.Client;

namespace WebApp.UI.Controllers
{
    public class BaseController : Controller
    {
        private readonly IAppClient _appClient;
        private readonly ApplicationOptions _applicationOptions;
        private readonly AuthenticationOptions _authenticationOptions;
        public BaseController(IAppClient appClient, ApplicationOptions applicationOptions, AuthenticationOptions authenticationOptions)
        {
            _appClient = appClient;
            _applicationOptions = applicationOptions;
            _authenticationOptions = authenticationOptions;
        }

        private static readonly string keyTokenCookie = "X-Access-Token";

        protected async Task StoreTokenInSession(object? value)
        {
            //STORE THE TOKEN AND REFRESH TOKEN IN THE SESSION
            var tokenDataString = Newtonsoft.Json.JsonConvert.SerializeObject(value);
            HttpContext.Session.SetString("X-Access-Token", tokenDataString);

            var tokenData = Newtonsoft.Json.JsonConvert.DeserializeObject<LoginResponseDto>(tokenDataString);

            if (tokenData != null)
            {
                await SetCurrentUserClaims(tokenData.Token);
            }
        }

        private async Task SetCurrentUserClaims(string token)
        {
            var result = await ValidateToken(token);

            if (result.isValid)
            {
                if (result.tokenValidationResult != null)
                {
                    if (result.tokenValidationResult.IsValid)
                    {
                        if (result.tokenValidationResult.Claims != null)
                        {
                            var claims = result.tokenValidationResult.Claims;

                            var firstName = claims.FirstOrDefault(x => x.Key == ClaimTypes.Name).Value.ToString() ?? string.Empty;
                            var email = claims.FirstOrDefault(x => x.Key == ClaimTypes.Email).Value.ToString() ?? string.Empty;
                            _ = int.TryParse(claims.FirstOrDefault(x => x.Key == ClaimTypes.Actor).Value.ToString(), out int userId);

                            CustomPrincipal userDetails = new CustomPrincipal(email)
                            {
                                Id = userId,
                                FullName = firstName,
                                Email = email,
                                Token = token
                            };

                            IPrincipal? user = userDetails;

                            HttpContext.Session.SetString("CurrentUserName", firstName);
                            HttpContext.Session.SetString("CurrentUserEmail", email);
                            HttpContext.Session.SetString("CurrentUserInitials", firstName.GetInitials());

                        }

                    }
                }
            }
        }

        private async Task<(bool isValid, TokenValidationResult tokenValidationResult)> ValidateToken(string token)
        {
            var validationResult = await GetTokenValidationResult(token);

            var _identity = validationResult?.ClaimsIdentity;

            if (_identity == null)
                return (false, validationResult);

            if (!_identity.IsAuthenticated)
                return (false, validationResult);

            return (true, validationResult);
        }

        private async Task<TokenValidationResult?> GetTokenValidationResult(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            if (!(tokenHandler.ReadToken(token) is JwtSecurityToken jwtToken)) return null;

            string secret = _authenticationOptions.JWTToken.Secret;

            var Key = Encoding.ASCII.GetBytes(secret);

            var symmetricKey = new SymmetricSecurityKey(Key);

            var validationParameters = new TokenValidationParameters()
            {
                RequireExpirationTime = true,
                ValidateIssuer = true,
                ValidateAudience = false,
                IssuerSigningKey = symmetricKey,
                ValidateIssuerSigningKey = true,
                ClockSkew = TimeSpan.FromMinutes(0),
                ValidIssuer = _authenticationOptions.JWTToken.Issuer

            };

            //ClaimsPrincipal principal = tokenHandler.ValidateToken(token, validationParameters, validatedToken: out SecurityToken securityToken);
            TokenValidationResult? result = await tokenHandler.ValidateTokenAsync(token, validationParameters);

            return result;
        }

        protected CustomPrincipal? CurrentUser
        {
            get
            {
                if (HttpContext != null)
                {
                    return HttpContext.User as CustomPrincipal;
                }
                else
                {
                    return new CustomPrincipal("");
                }
            }
        }

        protected int CurrentUserId
        {
            get
            {
                if (CurrentUser != null)
                {
                    return CurrentUser.Id;
                }
                else
                {
                    return 1;
                }
            }
        }

        protected string CurrentUserName
        {
            get
            {
                if (CurrentUser != null)
                {
                    return CurrentUser.FullName;
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        protected string CurrentUserEmail
        {
            get
            {
                if (CurrentUser != null)
                {
                    return CurrentUser.Email;
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        protected string CurrentUserInitials
        {
            get
            {
                if (CurrentUser != null)
                {
                    var result = CurrentUserName.GetInitials();
                    return result;
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        protected async Task BindCompanies()
        {
            ViewBag.Companies = null;

            var result = await _appClient.GetCompanyDropdownAsync();

            if (result.Success)
            {
                ViewBag.Companies = result.Data;
            }
        }

        protected async Task BindBranches()
        {
            ViewBag.Branches = null;

            var result = await _appClient.GetBranchDropdownAsync();

            if (result.Success)
            {
                ViewBag.Branches = result.Data;
            }
        }

        protected async Task BindDesignations()
        {
            ViewBag.Designations = null;

            var result = await _appClient.GetDesignationDropdownAsync();

            if (result.Success)
            {
                ViewBag.Designations = result.Data;
            }
        }

        protected async Task BindMasterDropdown()
        {
            ViewBag.Masters = null;

            var result = await _appClient.GetMasterDropdown();

            if (result.Success)
            {
                ViewBag.Masters = result.Data;
            }
        }
    }
}
