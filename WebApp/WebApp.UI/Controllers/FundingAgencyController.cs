using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text;
using WebApp.DTOs.FundingAgency.Request;
using WebApp.Global.Helpers;
using WebApp.Global.Options;
using WebApp.Global.Shared;
using WebApp.UI.Core.Proxy.Client;
using WebApp.UI.Models.FundingAgency;

namespace WebApp.UI.Controllers
{
    [Route("/fundingagency")]
    public class FundingAgencyController : BaseController
    {
        private readonly ILogger<FundingAgencyController> _logger;
        private readonly IAppClient _appClient;
        private readonly ApplicationOptions _applicationOptions;
        private readonly AuthenticationOptions _authenticationOptions;
        public FundingAgencyController(ILogger<FundingAgencyController> logger, IAppClient appClient, ApplicationOptions applicationOptions,
            AuthenticationOptions authenticationOptions)
            : base(appClient, applicationOptions, authenticationOptions)
        {
            _logger = logger;
            _appClient = appClient;
            _applicationOptions = applicationOptions;
            _authenticationOptions = authenticationOptions;
        }

        [HttpGet]
        [Route("", Name = "HolidayIndex")]
        public async Task<IActionResult> Index([FromQuery] SearchFundingAgency search, string srtex = "", int pno = 1, bool pg = false, bool srt = false, bool rst = false)
        {
            if (rst)
            {
                HttpContext.Session.SetString("Name", "");

                HttpContext.Session.SetString("SortExpression", "");
                HttpContext.Session.SetInt32("Page", pno);
            }

            int pageSize;
            SearchFundingAgencyRequestDto requestDto;

            PrepareSearchInputAndSetSessions(search, pg, srt, ref srtex, ref pno, out pageSize, out requestDto);

            var result = await _appClient.GetFundingAgencyWithPSS(requestDto);

            if (result.Success)
            {
                if (result.Data != null)
                {
                    int recsCount = result.Data.TotalRecords;

                    PagerHelper pager = new PagerHelper("FundingAgency", "Index", recsCount, pno, pageSize);

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

        private void PrepareSearchInputAndSetSessions(SearchFundingAgency search, bool paging, bool sorting, ref string sortExpression, ref int pageno, out int pageSize, out SearchFundingAgencyRequestDto requestDto)
        {
            PrepareSessionVariables(search, paging, sorting, ref sortExpression, ref pageno);

            SortingHelper sortingHelper = PrepareSortingVariables(sortExpression);

            pageSize = DefaultPagination.PageSize;

            requestDto = new SearchFundingAgencyRequestDto()
            {
                OrderBy = sortingHelper.OrderBy,
                OrderByDirection = sortingHelper.Direction,
                PageSize = pageSize,
                PageIndex = pageno,
                Name = search.name ?? ""
            };
        }

        private SortingHelper PrepareSortingVariables(string sortExpression)
        {
            SortingHelper sorting = new SortingHelper();

            sorting.AddColumn("Id", true);
            sorting.AddColumn("Name");
            sorting.AddColumn("ContactPerson");
            sorting.AddColumn("Email");
            sorting.AddColumn("Phone");
            sorting.AddColumn("Address");
            sorting.AddColumn("CreatedDate");
            sorting.ApplySort(sortExpression);

            ViewData["Sort"] = sorting;

            ViewBag.Pager = null;
            return sorting;
        }

        private void PrepareSessionVariables(SearchFundingAgency search, bool paging, bool sorting, ref string sortExpression, ref int pageno)
        {
            if (!string.IsNullOrWhiteSpace(search.name))
            {
                HttpContext.Session.SetString("Name", search.name);
            }
            else
            {
                if (paging || sorting)
                {
                    var name = HttpContext.Session.GetString("Name");

                    if (!string.IsNullOrWhiteSpace(name))
                    {
                        search.name = name;
                    }
                }

                HttpContext.Session.SetString("Name", search.name ?? "");
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
        [Route("{id}", Name = "AddOrUpdateFA")]
        public async Task<IActionResult> AddOrUpdateFA([FromRoute] int id)
        {
            if (id <= 0)
            {
                AddFundingAgencyRequestDto requestDto = new AddFundingAgencyRequestDto();
                return await Task.Run(() => PartialView("_AddFundingAgency", requestDto));
            }
            else
            {
                var fundingAgency = await _appClient.GetFundingAgencyDetailsAsync(id);
                UpdateFundingAgencyRequestDto requestDto = new UpdateFundingAgencyRequestDto();

                if (fundingAgency.Success)
                {
                    if (fundingAgency.Data != null)
                    {
                        requestDto.Id = fundingAgency.Data.Id;
                        requestDto.Name = fundingAgency.Data.Name;
                        requestDto.Description = fundingAgency.Data.Description;
                        requestDto.Address = fundingAgency.Data.Address;
                        requestDto.ContactPerson = fundingAgency.Data.ContactPerson;
                        requestDto.Email = fundingAgency.Data.Email;
                        requestDto.Phone = fundingAgency.Data.Phone;
                    }
                }

                return await Task.Run(() => PartialView("_UpdateFundingAgency", requestDto));
            }
        }

        [HttpPost]
        [Route("addfundingagency", Name = "AddFundingAgency")]
        public async Task<object> AddFundingAgency(AddFundingAgencyRequestDto requestDto)
        {
            StringBuilder sb = new StringBuilder();

            if (ModelState.IsValid)
            {
                var result = await _appClient.AddFundingAgencyAsync(requestDto);

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
        [Route("updatefundingagency", Name = "UpdateFundingAgency")]
        public async Task<object> UpdateFundingAgency(UpdateFundingAgencyRequestDto requestDto)
        {
            StringBuilder sb = new StringBuilder();

            if (ModelState.IsValid)
            {
                var result = await _appClient.UpdateFundingAgencyAsync(requestDto);

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
        [Route("{id}", Name = "DeleteFundingAgency")]
        public async Task<object> DeleteFundingAgency([FromRoute] int id)
        {
            StringBuilder sb = new StringBuilder();
            var result = await _appClient.DeleteFundingAgencyAsync(id);

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
        [Route("{id}", Name = "ChangeStatusFA")]
        public async Task<object> ChangeStatusFA([FromRoute] int id)
        {
            StringBuilder sb = new StringBuilder();
            var result = await _appClient.UpdateFundingAgencyStatusAsync(id);

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
