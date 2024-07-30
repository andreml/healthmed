using HealthMed.Application.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;

namespace HealthMed.CrossCutting.DI;

public class ValidateModelStateAttributeCollectionExtension : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        if (!context.ModelState.IsValid)
        {
            var errors = context.ModelState.Values.Where(v => v.Errors.Count > 0)
                    .SelectMany(v => v.Errors)
                    .Select(v => v.ErrorMessage)
                    .ToList();

            var errorResponse = new ErrorViewModel()
            {
                Messages = errors
            };

            context.Result = new JsonResult(errorResponse) { StatusCode = (int)HttpStatusCode.BadRequest };
        }
    }
}
