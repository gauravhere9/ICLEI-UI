using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text;
using WebApp.DTOs.User.Request;
using WebApp.Global.Helpers;
using WebApp.Global.Options;
using WebApp.Global.Shared;
using WebApp.UI.Core.Proxy.Client;
using WebApp.UI.Models.User;
using static WebApp.Global.Constants.Enumurations;

namespace WebApp.UI.Controllers
{
    [Route("/users")]
    public class UsersController : BaseController
    {
        private readonly ILogger<AuthController> _logger;
        private readonly IAppClient _appClient;
        private readonly ApplicationOptions _applicationOptions;
        private readonly AuthenticationOptions _authenticationOptions;
        public UsersController(ILogger<AuthController> logger, IAppClient appClient, ApplicationOptions applicationOptions,
            AuthenticationOptions authenticationOptions) : base(appClient, applicationOptions, authenticationOptions)
        {
            _logger = logger;
            _appClient = appClient;
            _applicationOptions = applicationOptions;
            _authenticationOptions = authenticationOptions;
        }

        [HttpGet]
        [Route("", Name = "UserIndex")]
        public async Task<IActionResult> Index([FromQuery] SearchUser searchUser, string srtex = "", int pno = 1, bool pg = false, bool srt = false, bool rst = false)
        {
            if (rst)
            {
                HttpContext.Session.SetString("Name", "");
                HttpContext.Session.SetString("EmployeeCode", "");
                HttpContext.Session.SetString("Email", "");
                HttpContext.Session.SetString("Mobile", "");
                HttpContext.Session.SetInt32("BranchId", 0);
                HttpContext.Session.SetInt32("DesignationId", 0);
                HttpContext.Session.SetInt32("UserTypeId", 0);

                HttpContext.Session.SetString("SortExpression", "");
                HttpContext.Session.SetInt32("Page", pno);
            }

            await BindBranches();

            await BindDesignations();

            await BindMasterDropdown();

            int pageSize;
            SearchSiteUserRequestDto requestDto;

            PrepareSearchInputAndSetSessions(searchUser, pg, srt, ref srtex, ref pno, out pageSize, out requestDto);

            var result = await _appClient.GetUserWithPSS(requestDto);

            if (result.Success)
            {
                if (result.Data != null)
                {
                    int recsCount = result.Data.TotalRecords;

                    PagerHelper pager = new PagerHelper("Users", "Index", recsCount, pno, pageSize);

                    ViewBag.Pager = pager;

                    return await Task.Run(() => View(result.Data));
                }
            }
            else
            {
                ViewBag.JavaScriptFunction = string.Format("ShowErrorSwal('{0}');", result.Message);
            }

            return await Task.Run(() => View());
        }

        private void PrepareSearchInputAndSetSessions(SearchUser searchUser, bool paging, bool sorting, ref string sortExpression, ref int pageno, out int pageSize, out SearchSiteUserRequestDto requestDto)
        {
            PrepareSessionVariables(searchUser, paging, sorting, ref sortExpression, ref pageno);

            SortingHelper sortingHelper = PrepareSortingVariables(sortExpression);

            pageSize = DefaultPagination.PageSize;

            requestDto = new SearchSiteUserRequestDto()
            {
                OrderBy = sortingHelper.OrderBy,
                OrderByDirection = sortingHelper.Direction,
                PageSize = pageSize,
                PageIndex = pageno,
                Name = searchUser.name ?? "",
                Email = searchUser.email ?? "",
                Mobile = searchUser.mobile ?? "",
                BranchId = searchUser.branchId ?? 0,
                DesignationId = searchUser.designationId ?? 0,
                UserTypeId = searchUser.userTypeId ?? 0,
                EmployeeCode = searchUser.employeecode ?? ""
            };
        }

        private SortingHelper PrepareSortingVariables(string sortExpression)
        {
            SortingHelper sorting = new SortingHelper();
            sorting.AddColumn("Name");
            sorting.AddColumn("EmployeeCode");
            sorting.AddColumn("Id", true);
            sorting.AddColumn("Email");
            sorting.AddColumn("Mobile");
            sorting.AddColumn("BranchName");
            sorting.AddColumn("DesignationName");
            sorting.AddColumn("UserTypeName");
            sorting.AddColumn("CreatedDate");
            sorting.ApplySort(sortExpression);

            ViewData["Sort"] = sorting;

            ViewBag.Pager = null;
            return sorting;
        }

        private void PrepareSessionVariables(SearchUser searchUser, bool paging, bool sorting, ref string sortExpression, ref int pageno)
        {
            if (!string.IsNullOrWhiteSpace(searchUser.name))
            {
                HttpContext.Session.SetString("Name", searchUser.name);
            }
            else
            {
                if (paging || sorting)
                {
                    var name = HttpContext.Session.GetString("Name");

                    if (!string.IsNullOrWhiteSpace(name))
                    {
                        searchUser.name = name;
                    }
                }

                HttpContext.Session.SetString("Name", searchUser.name ?? "");
            }

            if (!string.IsNullOrWhiteSpace(searchUser.employeecode))
            {
                HttpContext.Session.SetString("EmployeeCode", searchUser.employeecode);
            }
            else
            {
                if (paging || sorting)
                {
                    var employeecode = HttpContext.Session.GetString("EmployeeCode");

                    if (!string.IsNullOrWhiteSpace(employeecode))
                    {
                        searchUser.employeecode = employeecode;
                    }
                }

                HttpContext.Session.SetString("EmployeeCode", searchUser.employeecode ?? "");

            }

            if (!string.IsNullOrWhiteSpace(searchUser.email))
            {
                HttpContext.Session.SetString("Email", searchUser.email);
            }
            else
            {
                if (paging || sorting)
                {
                    var email = HttpContext.Session.GetString("Email");

                    if (!string.IsNullOrWhiteSpace(email))
                    {
                        searchUser.email = email;
                    }
                }

                HttpContext.Session.SetString("Email", searchUser.email ?? "");

            }

            if (!string.IsNullOrWhiteSpace(searchUser.mobile))
            {
                HttpContext.Session.SetString("Mobile", searchUser.mobile);
            }
            else
            {
                if (paging || sorting)
                {
                    var mobile = HttpContext.Session.GetString("Mobile");

                    if (!string.IsNullOrWhiteSpace(mobile))
                    {
                        searchUser.mobile = mobile;
                    }
                }

                HttpContext.Session.SetString("Mobile", searchUser.mobile ?? "");
            }

            if (searchUser.branchId > 0)
            {
                HttpContext.Session.SetInt32("BranchId", searchUser.branchId ?? 0);
            }
            else
            {
                if (paging || sorting)
                {
                    int.TryParse(HttpContext.Session.GetInt32("BranchId").ToString(), out int branchId);

                    if (branchId > 0)
                    {
                        searchUser.branchId = branchId;
                    }
                }

                HttpContext.Session.SetInt32("BranchId", searchUser.branchId ?? 0);
            }

            if (searchUser.designationId > 0)
            {
                HttpContext.Session.SetInt32("DesignationId", searchUser.designationId ?? 0);
            }
            else
            {
                if (paging || sorting)
                {
                    int.TryParse(HttpContext.Session.GetInt32("DesignationId").ToString(), out int designationId);

                    if (designationId > 0)
                    {
                        searchUser.designationId = designationId;
                    }
                }

                HttpContext.Session.SetInt32("DesignationId", searchUser.designationId ?? 0);

            }

            if (searchUser.userTypeId > 0)
            {
                HttpContext.Session.SetInt32("UserTypeId", searchUser.userTypeId ?? 0);
            }
            else
            {
                if (paging || sorting)
                {
                    int.TryParse(HttpContext.Session.GetInt32("UserTypeId").ToString(), out int userTypeId);

                    if (userTypeId > 0)
                    {
                        searchUser.userTypeId = userTypeId;
                    }
                }

                HttpContext.Session.SetInt32("UserTypeId", searchUser.userTypeId ?? 0);
            }

            if (!string.IsNullOrWhiteSpace(sortExpression))
            {
                HttpContext.Session.SetString("SortExpression", sortExpression);
            }
            else
            {
                var sortEx = HttpContext.Session.GetString("SortExpression");

                if (!string.IsNullOrWhiteSpace(sortEx))
                {
                    sortExpression = sortEx;
                }
                else
                {
                    HttpContext.Session.SetString("SortExpression", sortExpression);
                }
            }

            if (pageno > 0)
            {
                HttpContext.Session.SetInt32("Page", pageno);
            }
            else
            {
                var pId = HttpContext.Session.GetInt32("Page") ?? 0;

                pageno = pId <= 0 ? 1 : pageno;

                HttpContext.Session.SetInt32("Page", pageno);
            }
        }

        [HttpGet]
        [Route("add-user", Name = "AddUserLoad")]
        public async Task<IActionResult> AddUser()
        {
            await BindCompanies();
            await BindBranches();
            await BindDesignations();
            await BindMasterDropdown();

            AddSiteUserRequestDto requestDto = new AddSiteUserRequestDto();
            return await Task.Run(() => View(requestDto));
        }

        [HttpGet]
        [Route("{id}/update-user", Name = "UpdateUserLoad")]
        public async Task<IActionResult> UpdateUser([FromRoute] int id)
        {
            await BindCompanies();
            await BindBranches();
            await BindDesignations();
            await BindMasterDropdown();

            var user = await _appClient.GetUserDetailsAsync(id);
            UpdateSiteUserRequestDto requestDto = new UpdateSiteUserRequestDto();

            if (user.Success)
            {
                if (user.Data != null)
                {
                    requestDto.Id = user.Data.Id;
                    requestDto.Name = user.Data.Name;
                    requestDto.EmployeeCode = user.Data.EmployeeCode;
                    requestDto.Email = user.Data.Email;
                    requestDto.Mobile = user.Data.Mobile;
                    requestDto.PAN = user.Data.PAN;
                    requestDto.AadharNo = user.Data.AadharNo;
                    requestDto.BranchId = user.Data.BranchId;
                    requestDto.BloodGroupId = user.Data.BloodGroupId != null ? (BloodGroupTypes)user.Data.BloodGroupId : null;
                    requestDto.DesignationId = user.Data.DesignationId;
                    requestDto.DOB = user.Data.DOB;
                    requestDto.ReportingTo = user.Data.ReportingTo;
                    requestDto.EmergencyContact = user.Data.EmergencyContact;
                    requestDto.EmergencyPerson = user.Data.EmergencyPerson;
                    requestDto.GenderId = (GenderTypes)user.Data.GenderId;
                    requestDto.MaritalStatusId = user.Data.MaritalStatusId != null ? (MaritalStatus)user.Data.MaritalStatusId : null;
                    requestDto.SpouseName = user.Data.SpouseName;
                    requestDto.UserTypeId = (UserTypes)user.Data.UserTypeId;

                }
            }

            return await Task.Run(() => View(requestDto));
        }



        //[HttpGet]
        //[Route("{id}", Name = "AddOrUpdateUser")]
        //public async Task<IActionResult> AddOrUpdateUser([FromRoute] int id)
        //{
        //    await BindCompanies();
        //    await BindBranches();
        //    await BindDesignations();
        //    await BindMasterDropdown();

        //    if (id <= 0)
        //    {
        //        AddSiteUserRequestDto requestDto = new AddSiteUserRequestDto();
        //        return await Task.Run(() => PartialView("_AddUser", requestDto));
        //    }
        //    else
        //    {
        //        var user = await _appClient.GetUserDetailsAsync(id);
        //        UpdateSiteUserRequestDto requestDto = new UpdateSiteUserRequestDto();

        //        if (user.Success)
        //        {
        //            if (user.Data != null)
        //            {
        //                requestDto.Id = user.Data.Id;
        //                requestDto.Name = user.Data.Name;
        //                requestDto.EmployeeCode = user.Data.EmployeeCode;
        //                requestDto.Email = user.Data.Email;
        //                requestDto.Mobile = user.Data.Mobile;
        //                requestDto.PAN = user.Data.PAN;
        //                requestDto.AadharNo = user.Data.AadharNo;
        //                requestDto.BranchId = user.Data.BranchId;
        //                requestDto.BloodGroupId = user.Data.BloodGroupId != null ? (BloodGroupTypes)user.Data.BloodGroupId : null;
        //                requestDto.DesignationId = user.Data.DesignationId;
        //                requestDto.DOB = user.Data.DOB;
        //                requestDto.ReportingTo = user.Data.ReportingTo;
        //                requestDto.EmergencyContact = user.Data.EmergencyContact;
        //                requestDto.EmergencyPerson = user.Data.EmergencyPerson;
        //                requestDto.GenderId = (GenderTypes)user.Data.GenderId;
        //                requestDto.MaritalStatusId = user.Data.MaritalStatusId != null ? (MaritalStatus)user.Data.MaritalStatusId : null;
        //                requestDto.SpouseName = user.Data.SpouseName;
        //                requestDto.UserTypeId = (UserTypes)user.Data.UserTypeId;

        //            }
        //        }

        //        return await Task.Run(() => PartialView("_UpdateUser", requestDto));
        //    }
        //}

        [HttpPost]
        [Route("adduser", Name = "AddUser")]
        public async Task<object> AddUser(AddSiteUserRequestDto requestDto)
        {
            StringBuilder sb = new StringBuilder();

            if (ModelState.IsValid)
            {
                var result = await _appClient.AddUserAsync(requestDto);

                if (result.Success)
                {
                    return new { message = result.Message ?? "Success", statuscode = result.StatusCode };
                }
                else
                {
                    if (result.Errors != null)
                    {
                        foreach (var error in result.Errors)
                        {
                            sb.Append(error + ". ");
                        }
                    }
                    else
                    {
                        sb.Append("Something went wrong. Try Again Later.");
                    }

                    return new { message = sb.ToString() ?? "Failed", statuscode = result.StatusCode };
                }
            }
            else
            {
                sb.Append("Failed");
            }

            return new { message = sb.ToString() ?? "Failed", statuscode = (int)HttpStatusCode.BadRequest };
        }

        [HttpPut]
        [Route("updateuser", Name = "UpdateUser")]
        public async Task<object> UpdateUser(UpdateSiteUserRequestDto requestDto)
        {
            StringBuilder sb = new StringBuilder();

            if (ModelState.IsValid)
            {
                var result = await _appClient.UpdateUserAsync(requestDto);

                if (result.Success)
                {
                    return new { message = result.Message ?? "Success", statuscode = result.StatusCode };
                }
                else
                {
                    if (result.Errors != null)
                    {
                        foreach (var error in result.Errors)
                        {
                            sb.Append(error + ". ");
                        }
                    }
                    else
                    {
                        sb.Append("Something went wrong. Try Again Later.");
                    }

                    return new { message = sb.ToString() ?? "Failed", statuscode = result.StatusCode };
                }
            }
            else
            {
                sb.Append("Failed");
            }

            return new { message = sb.ToString() ?? "Failed", statuscode = (int)HttpStatusCode.BadRequest };
        }

        [HttpDelete]
        [Route("{id}", Name = "DeleteUser")]
        public async Task<object> DeleteUser([FromRoute] int id)
        {
            StringBuilder sb = new StringBuilder();
            var result = await _appClient.DeleteUserAsync(id);

            if (!result.Success)
            {
                if (result.Errors != null)
                {
                    foreach (var error in result.Errors)
                    {
                        sb.Append(error + ". ");
                    }
                }
                else
                {
                    sb.Append("Something went wrong. Try Again Later.");
                }

                return new { message = sb.ToString() ?? "Failed", statuscode = (int)HttpStatusCode.BadRequest };
            }

            return new { message = result.Message ?? "Success", statuscode = result.StatusCode };
        }

        [HttpPatch]
        [Route("{id}", Name = "ChangeUserStatus")]
        public async Task<object> ChangeUserStatus([FromRoute] int id)
        {
            StringBuilder sb = new StringBuilder();
            var result = await _appClient.UpdateUserStatusAsync(id);

            if (!result.Success)
            {
                if (result.Errors != null)
                {
                    foreach (var error in result.Errors)
                    {
                        sb.Append(error + ". ");
                    }
                }
                else
                {
                    sb.Append("Something went wrong. Try Again Later.");
                }

                return new { message = sb.ToString() ?? "Failed", statuscode = (int)HttpStatusCode.BadRequest };
            }

            return new { message = result.Message ?? "Success", statuscode = result.StatusCode };
        }

        [HttpGet]
        [Route("{branchId}/reporting-to", Name = "BindReportingTo")]
        public async Task<object> BindReportingToDropdown([FromRoute] int branchId)
        {
            ViewBag.ReportingTo = null;

            var result = await _appClient.GetUserDropdownAsync(branchId);

            if (result.Success)
            {
                ViewBag.ReportingTo = result.Data;
            }

            return result;

        }
    }
}
