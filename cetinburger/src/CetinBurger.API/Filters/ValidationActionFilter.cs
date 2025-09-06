using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CetinBurger.API.Filters;

// FluentValidation ile doğrulanan ModelState hatalarını RFC7807 ProblemDetails formatında 400 olarak döndürür.
public class ValidationActionFilter : IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        if (!context.ModelState.IsValid)
        {
            var problem = new ValidationProblemDetails(context.ModelState)
            {
                Title = "One or more validation errors occurred.",
                Status = StatusCodes.Status400BadRequest,
                Type = "https://httpstatuses.com/400"
            };
            context.Result = new BadRequestObjectResult(problem);
            return;
        }

        await next();
    }
}


