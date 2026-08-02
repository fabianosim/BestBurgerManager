using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BestBurgerManagerAPI.ApiResponses
{
    /// <summary>
    /// Class to handle BadRequest responses.
    /// </summary>
    public class ApiBadRequestResponse : ApiResponse
    {
        /// <summary>
        /// A dictionary containing the errors from the called methods.
        /// </summary>
        public Dictionary<string, string> Errors { get; }

        /// <summary>
        /// Prepare a BadRequest (HTTP error 400) to be returned to client.
        /// </summary>
        /// <param name="modelState">The model state containing the errors.</param>
        public ApiBadRequestResponse(ModelStateDictionary modelState)
            : base(400)
        {
            if (modelState.IsValid)
            {
                throw new ArgumentException("ModelState must be invalid", nameof(modelState));
            }

            Errors = modelState.ToDictionary(x => x.Key, x => (x.Value.Errors.FirstOrDefault() == null ? string.Empty : x.Value.Errors.FirstOrDefault().ErrorMessage));
        }
    }
}
