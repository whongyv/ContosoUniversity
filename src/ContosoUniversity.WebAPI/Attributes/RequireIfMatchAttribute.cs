using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Primitives;

namespace ContosoUniversity.WebAPI.Attributes
{
    public class RequireIfMatchAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var ifMatch = context.HttpContext.Request.Headers.IfMatch;
            if (StringValues.IsNullOrEmpty(ifMatch))
            {
                var problemDetails = new ProblemDetails
                {
                    Status = StatusCodes.Status428PreconditionRequired,
                    Title = "Precondition required",
                    Detail = "This operation requires an If-Match header containing the resource's ETag.",
                    Instance = context.HttpContext.Request.Path
                };
                context.Result = new ObjectResult(problemDetails)
                {
                    StatusCode = StatusCodes.Status428PreconditionRequired
                };
            }
        }
    }
}
