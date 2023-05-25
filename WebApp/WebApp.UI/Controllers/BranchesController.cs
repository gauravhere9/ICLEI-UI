using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text;
using WebApp.DTOs.Branches.Request;
using WebApp.Global.Helpers;
using WebApp.Global.Options;
using WebApp.UI.Core.Proxy.Client;

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
        [Route("")]
        public async Task<IActionResult> Index(string sortExpression = "")
        {
            SortingHelper sorting = new SortingHelper();
            sorting.AddColumn("CompanyName");
            sorting.AddColumn("BranchCode", true);
            sorting.AddColumn("Location");
            sorting.AddColumn("Address");
            sorting.AddColumn("CreatedDate");
            sorting.ApplySort(sortExpression);

            ViewData["Sort"] = sorting;

            BranchSearchRequestDto requestDto = new BranchSearchRequestDto()
            {
                OrderBy = sorting.OrderBy,
                OrderByDirection = sorting.Direction
            };

            var result = await _appClient.GetBranchesWithPSS(requestDto);

            if (result.Success)
            {
                if (result.Data != null)
                {
                    return await Task.Run(() => View(result.Data));
                }
            }
            else
            {
                ViewBag.JavaScriptFunction = string.Format("ShowErrorSwal('{0}');", result.Message);
            }

            return await Task.Run(() => View());
        }



        [HttpGet]
        [Route("{id}", Name = "AddOrUpdate")]
        public async Task<IActionResult> AddOrUpdate([FromRoute] int id)
        {
            if (id <= 0)
            {
                AddBranchRequestDto requestDto = new AddBranchRequestDto();

                ViewBag.Companies = null;

                var companies = await _appClient.GetCompanyDropdownAsync();

                if (companies.Success)
                {
                    ViewBag.Companies = companies.Data;
                }

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
