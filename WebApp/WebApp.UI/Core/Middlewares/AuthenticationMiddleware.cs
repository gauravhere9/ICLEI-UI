using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using WebApp.DTOs.Auth.Login.Response;
using WebApp.Global.Options;
using WebApp.Global.Shared;

namespace WebApp.UI.Core.Middlewares
{
    /// <summary>
    /// 
    /// </summary>
    public class AuthenticationMiddleware
    {
        private readonly RequestDelegate _next;

        private readonly AuthenticationOptions _authenticationOptions;
        private static ClaimsIdentity? _identity;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="next"></param>
        /// <param name="authenticationOptions"></param>
        /// <param name="userService"></param>
        public AuthenticationMiddleware(RequestDelegate next, AuthenticationOptions authenticationOptions)
        {
            _next = next;
            _authenticationOptions = authenticationOptions;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task Invoke(HttpContext context)
        {
            if (_authenticationOptions.Enabled)
            {
                var key = "X-Access-Token";
                var tokenDetails = context.Request.Cookies[key];

                var endpoint = context.GetEndpoint();
                if (endpoint == null)
                {
                    if (string.IsNullOrWhiteSpace(tokenDetails))
                    {
                        context.Response.Redirect("/");
                        await Task.CompletedTask;
                        return;
                    }
                    else
                    {
                        context.Response.Redirect("/dashboard");
                        await Task.CompletedTask;
                        return;
                    }
                    //await _next(context);
                }

                if (endpoint?.Metadata?.GetMetadata<IAllowAnonymous>() is object)
                {
                    if (string.IsNullOrWhiteSpace(tokenDetails))
                    {
                        await _next(context);
                        await Task.CompletedTask;
                        return;
                    }
                    else
                    {
                        context.Response.Redirect("/dashboard");
                        await Task.CompletedTask;
                        return;
                    }
                }

                if (!string.IsNullOrWhiteSpace(tokenDetails))
                {
                    var loginResponseDto = Newtonsoft.Json.JsonConvert.DeserializeObject<LoginResponseDto>(tokenDetails);

                    if (loginResponseDto != null)
                    {
                        if (!string.IsNullOrWhiteSpace(loginResponseDto.Token))
                        {
                            var principal = await AuthenticateJwtToken(loginResponseDto.Token);

                            if (principal != null)
                            {
                                context.User = principal as ClaimsPrincipal;

                                if (context.User?.Identity?.IsAuthenticated == true)
                                {
                                    await _next.Invoke(context);
                                    await Task.CompletedTask;
                                    return;
                                }
                            }
                        }
                    }

                    context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    context.Response.Redirect("/401-unauthorized");
                    await Task.CompletedTask;
                    return;
                }
                else
                {
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    context.Response.Redirect("/");
                    await Task.CompletedTask;
                    return;
                }
            }
            else
            {
                context.Response.StatusCode = (int)HttpStatusCode.OK;
                await _next.Invoke(context);
                await Task.CompletedTask;
                return;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        protected async Task<IPrincipal?> AuthenticateJwtToken(string token)
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
                            int.TryParse(claims.FirstOrDefault(x => x.Key == ClaimTypes.Actor).Value.ToString(), out int userId);

                            CustomPrincipal userDetails = new CustomPrincipal(email)
                            {
                                Id = userId,
                                FullName = firstName,
                                Email = email,
                                Token = token
                            };

                            IPrincipal? user = userDetails;
                            return user;
                        }

                    }
                }
            }
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<(bool isValid, TokenValidationResult tokenValidationResult)> ValidateToken(string token)
        {
            var validationResult = await GetTokenValidationResult(token);

            _identity = validationResult?.ClaimsIdentity;

            if (_identity == null)
                return (false, validationResult);

            if (!_identity.IsAuthenticated)
                return (false, validationResult);

            return (true, validationResult);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<TokenValidationResult?> GetTokenValidationResult(string token)
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

    }

    /// <summary>
    /// 
    /// </summary>
    public static class AuthenticationMiddlewareExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseAuthenticationMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<AuthenticationMiddleware>();
        }
    }
}
