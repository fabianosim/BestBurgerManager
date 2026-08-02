using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BestBurgerManagerAPI.ApiResponses
{
    /// <summary>
    /// Class to handle filter validations from the calling methods.
    /// </summary>
    public class ApiValidationFilter : ActionFilterAttribute
    {
        /// <summary>
        /// Event called when a validation is being executed.
        /// </summary>
        /// <param name="context"></param>
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                context.Result = new BadRequestObjectResult(new ApiBadRequestResponse(context.ModelState));
            }

            base.OnActionExecuting(context);
        }
    }
}
