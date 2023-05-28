using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text;
using WebApp.DTOs.Designation.Request;
using WebApp.Global.Helpers;
using WebApp.Global.Options;
using WebApp.Global.Shared;
using WebApp.UI.Core.Proxy.Client;
using WebApp.UI.Models.Designation;

namespace WebApp.UI.Controllers
{
    [Route("/designations")]
    public class DesignationsController : BaseController
    {
        private readonly ILogger<AuthController> _logger;
        private readonly IAppClient _appClient;
        private readonly ApplicationOptions _applicationOptions;
        private readonly AuthenticationOptions _authenticationOptions;
        public DesignationsController(ILogger<AuthController> logger, IAppClient appClient, ApplicationOptions applicationOptions, AuthenticationOptions authenticationOptions) : base(applicationOptions, authenticationOptions)
        {
            _logger = logger;
            _appClient = appClient;
            _applicationOptions = applicationOptions;
            _authenticationOptions = authenticationOptions;
        }

        [HttpGet]
        [Route("", Name = "DesignationIndex")]
        public async Task<IActionResult> Index([FromQuery] SearchDesignation searchDesignation, string srtex = "", int pno = 1, bool pg = false, bool srt = false, bool rst = false)
        {
            if (rst)
            {
                HttpContext.Session.SetString("Name", "");
                HttpContext.Session.SetString("SortExpression", "");
                HttpContext.Session.SetInt32("Page", pno);
            }

            int pageSize;
            DesignationSearchRequestDto requestDto;

            PrepareSearchInputAndSetSessions(searchDesignation, pg, srt, ref srtex, ref pno, out pageSize, out requestDto);

            var result = await _appClient.GetDesignationesWithPSS(requestDto);

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

        private void PrepareSearchInputAndSetSessions(SearchDesignation searchDesignation, bool paging, bool sorting, ref string sortExpression, ref int pageno, out int pageSize, out DesignationSearchRequestDto requestDto)
        {
            PrepareSessionVariables(searchDesignation, paging, sorting, ref sortExpression, ref pageno);

            SortingHelper sortingHelper = PrepareSortingVariables(sortExpression);

            pageSize = DefaultPagination.PageSize;

            requestDto = new DesignationSearchRequestDto()
            {
                OrderBy = sortingHelper.OrderBy,
                OrderByDirection = sortingHelper.Direction,
                PageSize = pageSize,
                PageIndex = pageno,
                Name = searchDesignation.name ?? ""
            };
        }

        private SortingHelper PrepareSortingVariables(string sortExpression)
        {
            SortingHelper sorting = new SortingHelper();
            sorting.AddColumn("Id", true);
            sorting.AddColumn("Name");
            sorting.AddColumn("Description");
            sorting.AddColumn("CreatedDate");
            sorting.ApplySort(sortExpression);

            ViewData["Sort"] = sorting;

            ViewBag.Pager = null;
            return sorting;
        }

        private void PrepareSessionVariables(SearchDesignation searchDesignation, bool paging, bool sorting, ref string sortExpression, ref int pageno)
        {
            if (!string.IsNullOrWhiteSpace(searchDesignation.name))
            {
                HttpContext.Session.SetString("Name", searchDesignation.name);
            }
            else
            {
                if (paging || sorting)
                {
                    var bCode = HttpContext.Session.GetString("Name");

                    if (!string.IsNullOrWhiteSpace(bCode))
                    {
                        searchDesignation.name = bCode;
                    }
                }

                HttpContext.Session.SetString("Name", searchDesignation.name ?? "");
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
        [Route("{id}", Name = "AddOrUpdateDesignation")]
        public async Task<IActionResult> AddOrUpdateDesignation([FromRoute] int id)
        {
            ViewBag.Companies = null;

            var companies = await _appClient.GetCompanyDropdownAsync();

            if (companies.Success)
            {
                ViewBag.Companies = companies.Data;
            }

            if (id <= 0)
            {
                AddDesignationRequestDto requestDto = new AddDesignationRequestDto();
                return await Task.Run(() => PartialView("_AddDesignation", requestDto));
            }
            else
            {
                var designation = await _appClient.GetDesignationDetailsAsync(id);
                UpdateDesignationRequestDto requestDto = new UpdateDesignationRequestDto();

                if (designation.Success)
                {
                    if (designation.Data != null)
                    {
                        requestDto.Id = designation.Data.Id;
                        requestDto.Name = designation.Data.Name;
                        requestDto.Description = designation.Data.Description;
                    }
                }

                return await Task.Run(() => PartialView("_UpdateDesignation", requestDto));
            }
        }

        [HttpPost]
        [Route("adddesignation", Name = "AddDesignation")]
        public async Task<object> AddDesignation(AddDesignationRequestDto requestDto)
        {
            StringBuilder sb = new StringBuilder();

            if (ModelState.IsValid)
            {
                var result = await _appClient.AddDesignationAsync(requestDto);

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
        [Route("updatedesignation", Name = "UpdateDesignation")]
        public async Task<object> UpdateDesignation(UpdateDesignationRequestDto requestDto)
        {
            StringBuilder sb = new StringBuilder();

            if (ModelState.IsValid)
            {
                var result = await _appClient.UpdateDesignationAsync(requestDto);

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
        [Route("{id}", Name = "DeleteDesignation")]
        public async Task<object> DeleteDesignation([FromRoute] int id)
        {
            StringBuilder sb = new StringBuilder();
            var result = await _appClient.DeleteDesignationAsync(id);

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
        [Route("{id}", Name = "ChangeDesignationStatus")]
        public async Task<object> ChangeDesignationStatus([FromRoute] int id)
        {
            StringBuilder sb = new StringBuilder();
            var result = await _appClient.UpdateDesignationStatusAsync(id);

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
