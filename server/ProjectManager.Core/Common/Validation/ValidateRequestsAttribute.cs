using Microsoft.AspNetCore.Mvc.Filters;
using ProjectManager.Common.Exceptions;

namespace ProjectManager.Common.Validation;


public class ValidateRequestsAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
        => ProjectManagerBadRequestException.ThrowIfInvalid(context.ModelState);
}