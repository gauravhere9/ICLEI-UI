using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text;
using WebApp.DTOs.Branches.Request;
using WebApp.Global.Helpers;
using WebApp.Global.Options;
using WebApp.Global.Shared;
using WebApp.UI.Core.Proxy.Client;
using WebApp.UI.Models.Branch;

namespace WebApp.UI.Controllers
{
    [Route("/branches")]
    public class BranchesController : BaseController
    {
        private readonly ILogger<AuthController> _logger;
        private readonly IAppClient _appClient;
        private readonly ApplicationOptions _applicationOptions;
        private readonly AuthenticationOptions _authenticationOptions;
        public BranchesController(ILogger<AuthController> logger, IAppClient appClient, ApplicationOptions applicationOptions, AuthenticationOptions authenticationOptions) : base(applicationOptions, authenticationOptions)
        {
            _logger = logger;
            _appClient = appClient;
            _applicationOptions = applicationOptions;
            _authenticationOptions = authenticationOptions;
        }

        [HttpGet]
        [Route("", Name = "Index")]
        public async Task<IActionResult> Index([FromQuery] SearchBranch searchBranch, string srtex = "", int pno = 1, bool pg = false, bool srt = false, bool rst = false)
        {
            if (rst)
            {
                HttpContext.Session.SetString("BranchCode", "");
                HttpContext.Session.SetString("Location", "");
                HttpContext.Session.SetString("SortExpression", "");
                HttpContext.Session.SetInt32("CompanyId", 0);
                HttpContext.Session.SetInt32("Page", pno);
            }

            await BindCompanies();
            int pageSize;
            BranchSearchRequestDto requestDto;

            PrepareSearchInputAndSetSessions(searchBranch, pg, srt, ref srtex, ref pno, out pageSize, out requestDto);

            var result = await _appClient.GetBranchesWithPSS(requestDto);

            if (result.Success)
            {
                if (result.Data != null)
                {
                    int recsCount = result.Data.TotalRecords;

                    PagerHelper pager = new PagerHelper("Branches", "Index", recsCount, pno, pageSize);

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

        private void PrepareSearchInputAndSetSessions(SearchBranch searchBranch, bool paging, bool sorting, ref string sortExpression, ref int pageno, out int pageSize, out BranchSearchRequestDto requestDto)
        {
            PrepareSessionVariables(searchBranch, paging, sorting, ref sortExpression, ref pageno);

            SortingHelper sortingHelper = PrepareSortingVariables(sortExpression);

            pageSize = DefaultPagination.PageSize;

            requestDto = new BranchSearchRequestDto()
            {
                OrderBy = sortingHelper.OrderBy,
                OrderByDirection = sortingHelper.Direction,
                PageSize = pageSize,
                PageIndex = pageno,
                BranchCode = searchBranch.bcode,
                Location = searchBranch.loc,
                CompanyId = searchBranch.comp ?? 0

            };
        }

        private SortingHelper PrepareSortingVariables(string sortExpression)
        {
            SortingHelper sorting = new SortingHelper();
            sorting.AddColumn("CompanyName");
            sorting.AddColumn("Id", true);
            sorting.AddColumn("BranchCode");
            sorting.AddColumn("Location");
            sorting.AddColumn("Address");
            sorting.AddColumn("CreatedDate");
            sorting.ApplySort(sortExpression);

            ViewData["Sort"] = sorting;

            ViewBag.Pager = null;
            return sorting;
        }

        private void PrepareSessionVariables(SearchBranch searchBranch, bool paging, bool sorting, ref string sortExpression, ref int pageno)
        {
            if (!string.IsNullOrWhiteSpace(searchBranch.bcode))
            {
                HttpContext.Session.SetString("BranchCode", searchBranch.bcode);
            }
            else
            {
                if (paging || sorting)
                {
                    var bCode = HttpContext.Session.GetString("BranchCode");

                    if (!string.IsNullOrWhiteSpace(bCode))
                    {
                        searchBranch.bcode = bCode;
                    }
                }

                HttpContext.Session.SetString("BranchCode", searchBranch.bcode ?? "");
            }


            if (!string.IsNullOrWhiteSpace(searchBranch.loc))
            {
                HttpContext.Session.SetString("Location", searchBranch.loc);
            }
            else
            {
                if (paging || sorting)
                {
                    var location = HttpContext.Session.GetString("Location");

                    if (!string.IsNullOrWhiteSpace(location))
                    {
                        searchBranch.loc = location;
                    }
                }

                HttpContext.Session.SetString("Location", searchBranch.loc ?? "");

            }


            if (searchBranch.comp > 0)
            {
                HttpContext.Session.SetInt32("CompanyId", searchBranch.comp ?? 0);
            }
            else
            {
                if (paging || sorting)
                {
                    var cId = HttpContext.Session.GetInt32("CompanyId");

                    if (cId > 0)
                    {
                        searchBranch.comp = cId;
                    }
                }
                HttpContext.Session.SetInt32("CompanyId", searchBranch.comp ?? 0);
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

        private async Task BindCompanies()
        {
            ViewBag.Companies = null;

            var companies = await _appClient.GetCompanyDropdownAsync();

            if (companies.Success)
            {
                ViewBag.Companies = companies.Data;
            }
        }

        [HttpGet]
        [Route("{id}", Name = "AddOrUpdate")]
        public async Task<IActionResult> AddOrUpdate([FromRoute] int id)
        {
            ViewBag.Companies = null;

            var companies = await _appClient.GetCompanyDropdownAsync();

            if (companies.Success)
            {
                ViewBag.Companies = companies.Data;
            }

            if (id <= 0)
            {
                AddBranchRequestDto requestDto = new AddBranchRequestDto();
                return await Task.Run(() => PartialView("_AddBranch", requestDto));
            }
            else
            {
                var branch = await _appClient.GetBranchDetailsAsync(id);
                UpdateBranchRequestDto requestDto = new UpdateBranchRequestDto();

                if (branch.Success)
                {
                    if (branch.Data != null)
                    {
                        requestDto.Id = branch.Data.Id;
                        requestDto.BranchCode = branch.Data.BranchCode;
                        requestDto.Address = branch.Data.Address;
                        requestDto.Location = branch.Data.Location;
                        requestDto.CompanyId = branch.Data.CompanyId;
                    }
                }

                return await Task.Run(() => PartialView("_UpdateBranch", requestDto));
            }
        }

        [HttpPost]
        [Route("addbranch", Name = "AddBranch")]
        public async Task<object> AddBranch(AddBranchRequestDto requestDto)
        {
            StringBuilder sb = new StringBuilder();

            if (ModelState.IsValid)
            {
                var result = await _appClient.AddBranchAsync(requestDto);

                if (result.Success)
                {
                    //ViewBag.JavaScriptFunction = string.Format("ShowSuccessSwal('{0}', '/branches');", result.Message);

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
        [Route("updatebranch", Name = "UpdateBranch")]
        public async Task<object> UpdateBranch(UpdateBranchRequestDto requestDto)
        {
            StringBuilder sb = new StringBuilder();

            if (ModelState.IsValid)
            {
                var result = await _appClient.UpdateBranchAsync(requestDto);

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
        [Route("{id}", Name = "DeleteBranch")]
        public async Task<object> DeleteBranch([FromRoute] int id)
        {
            StringBuilder sb = new StringBuilder();
            var result = await _appClient.DeleteBranchAsync(id);

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
        [Route("{id}", Name = "ChangeStatus")]
        public async Task<object> ChangeStatus([FromRoute] int id)
        {
            StringBuilder sb = new StringBuilder();
            var result = await _appClient.UpdateBranchStatusAsync(id);

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
    }
}
