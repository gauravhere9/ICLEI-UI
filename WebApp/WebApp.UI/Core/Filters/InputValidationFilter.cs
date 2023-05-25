using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;
using WebApp.Global.Constants;
using WebApp.Global.Extensions;

namespace WebApp.UI.Core.Filters
{
    public class InputValidationFilter : IAsyncActionFilter
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="filterContext"></param>
        public async Task OnActionExecutionAsync(ActionExecutingContext context,
            ActionExecutionDelegate next)
        {//To do : before the action executes  
            if (!context.ModelState.IsValid)
            {

                var errors = context.ModelState.Values.Where(v => v.Errors.Count > 0)
                    .SelectMany(v => v.Errors)
                    .Select(v => v.ErrorMessage)
                    .OrderBy(o => o)
                    .ToList();

                //ViewBag.JavaScriptFunction = string.Format("ShowErrorSwal('{0}');", ResponseStaticMessages.INVALID_INPUT);
                //context.Result = string.Format("ShowErrorSwal('{0}');", ResponseStaticMessages.INVALID_INPUT);

                Controller? controller = context.Controller as Controller;
                context.Result = controller?.ViewBag.JavaScriptFunction(string.Format("ShowErrorSwal('{0}');", ResponseStaticMessages.INVALID_INPUT)) as IActionResult;

                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.PreconditionFailed;
                return;
            }

            if (context.ActionArguments.Count > 0)
            {
                context.ActionArguments.Values.First().ReformatString();
            }


            await next();
            //To do : after the action executes  
        }
    }
}
