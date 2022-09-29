using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;

namespace noCarbon.API.Infrastracture.Filters
{
    /// <summary>
    /// 
    /// </summary>
    public class ValidationFilter : ActionFilterAttribute, IActionFilter
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                var result = new Dictionary<string, string>();
                foreach (var key in context.ModelState.Keys)
                {
                    result.Add(key, String.Join(", ", context.ModelState[key].Errors.Select(p => p.ErrorMessage)));
                }
                var responseModel = Response<Dictionary<string, string>>.Fail("Invalid data");
                responseModel.Data = result;
                context.Result = new ObjectResult(responseModel) { StatusCode = (int)HttpStatusCode.BadRequest};
            }
        }
    }
}
