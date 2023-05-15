using Microsoft.AspNetCore.Mvc.Filters;

namespace WebApp.UI.Core.Filters
{
    [AttributeUsage(AttributeTargets.All)]
    public class CustomAuthorizationFilter : Attribute, IAuthorizationFilter
    {
        //private readonly int _modules;
        //private readonly int _pages;
        //private readonly List<int> _permissions;
        //private IUserService _userService;
        //private readonly AuthenticationOptions _authenticationOptions;

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="userService"></param>
        ///// <param name="authenticationOptions"></param>
        //public CustomAuthorizationFilter(IUserService userService, AuthenticationOptions authenticationOptions)
        //{
        //    _authenticationOptions = authenticationOptions;
        //    _userService = userService;
        //}

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="modules"></param>
        ///// <param name="pages"></param>
        ///// <param name="permissions"></param>
        //public CustomAuthorizationFilter(int modules, int pages, int[] permissions)
        //{
        //    _modules = modules;
        //    _pages = pages;
        //    _permissions = new List<int>();
        //    _permissions.AddRange(permissions);
        //}

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="context"></param>
        //public void OnAuthorization(AuthorizationFilterContext context)
        //{
        //    if (context != null)
        //    {
        //        if (_authenticationOptions.Enabled)
        //        {
        //            var user = context.HttpContext.User as CustomPrincipal;

        //            if (user != null)
        //            {
        //                if (user.Identity.IsAuthenticated)
        //                {
        //                    _userService = (IUserService)context.HttpContext.RequestServices.GetService(typeof(IUserService));

        //                    if (_userService != null)
        //                    {
        //                        //GET THE USER PERMISSIONS
        //                        var permissionsResult = _userService.GetUserPermissions(user.Id);

        //                        //var permissions = permissionsResult.Result as Response;
        //                        var permissions = permissionsResult.Result as ApiResponse;

        //                        if (permissions.StatusCode == (int)HttpStatusCode.OK)
        //                        {
        //                            if (permissions.Data is List<PermissionModulesResponseDto> userPermissions)
        //                            {
        //                                if (userPermissions != null)
        //                                {
        //                                    var userPerm = userPermissions
        //                                        .Where(x => x.ModuleId == _modules)
        //                                        .SingleOrDefault().Pages;

        //                                    if (userPerm != null)
        //                                    {
        //                                        var userPermPages = userPerm.Where(x => x.PageId == _pages).SingleOrDefault();

        //                                        if (userPermPages != null)
        //                                        {
        //                                            if (userPermPages.PermissionTypes.Any(x => _permissions.Contains(x.PermissionTypeId)))
        //                                            {
        //                                                context.HttpContext.Response.Headers.Add("AuthStatus", "Authorized");
        //                                                return;
        //                                            }
        //                                        }
        //                                    }
        //                                }
        //                            }
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //        else
        //        {
        //            return;
        //        }
        //    }

        //    var response = new ApiResponse((int)HttpStatusCode.Unauthorized);
        //    context.HttpContext.Response.ContentType = "application/json";
        //    context.HttpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
        //    context.Result = new JsonResult(response);
        //}
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            throw new NotImplementedException();
        }
    }
}
