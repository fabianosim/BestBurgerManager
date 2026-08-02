using BestBurgerManager.Business.Extenstions.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BestBurgerManagerAPI.ApiResponses.Support
{
    /// <summary>
    /// THis class holds some support methods to handle request responses to controller methods.
    /// </summary>
    public static class RequestResponses
    {
        /// <summary>
        /// Add an error to the ModelState if one of the below exceptions is caught.
        /// </summary>
        /// <param name="ex">The exception object to be tested.</param>
        /// <param name="modelState">The current ModelState reference.</param>
        /// <returns>Returns an object containing a Bad Request response to the client.</returns>
        public static ApiBadRequestResponse RaiseKitchenOperationError(Exception ex, ModelStateDictionary modelState)
        {
            if (ex is ProductNotFoundException)
                modelState.AddModelError(ErrorKeys.ProductNotFound.ToString(), ex.Message);
            else if (ex is OrderNotFoundException)
                modelState.AddModelError(ErrorKeys.OrderNotFound.ToString(), ex.Message);
            else if (ex is OrderNotFullyReadyException)
                modelState.AddModelError(ErrorKeys.OrderNotFullyReady.ToString(), ex.Message);
            else
                modelState.AddModelError(ErrorKeys.UnhandledError.ToString(), ex.Message);

            // If the Model State is invalid, then one of the catch blocks above has been touched. An error will be returned.
            if (!modelState.IsValid)
            {
                return new ApiBadRequestResponse(modelState);
            }

            return null;
        }

        /// <summary>
        /// Add an error to the ModelState if one of the below exceptions is caught.
        /// </summary>
        /// <param name="ex">The exception object to be tested.</param>
        /// <param name="modelState">The current ModelState reference.</param>
        /// <returns>Returns an object containing a Bad Request response to the client.</returns>
        public static ApiBadRequestResponse RaiseOrderOperationError(Exception ex, ModelStateDictionary modelState)
        {
            if (ex is JsonSerializationException)
                modelState.AddModelError(ErrorKeys.InvalidObjectType.ToString(), ex.Message);
            else if (ex is OrderNotFoundException)
                modelState.AddModelError(ErrorKeys.OrderNotFound.ToString(), ex.Message);
            else if (ex is OrderInvalidPosIdException)
                modelState.AddModelError(ErrorKeys.InvalidPOSId.ToString(), ex.Message);
            else if (ex is OrderInvalidStatusException)
                modelState.AddModelError(ErrorKeys.InvalidStatusId.ToString(), ex.Message);
            else if (ex is OrderWithNoItemsException)
                modelState.AddModelError(ErrorKeys.NoItemsInOrder.ToString(), ex.Message);
            else if (ex is UserNotFoundException)
                modelState.AddModelError(ErrorKeys.UserNotFound.ToString(), ex.Message);
            else
                modelState.AddModelError(ErrorKeys.UnhandledError.ToString(), ex.Message);

            // If the Model State is invalid, then one of the catch blocks above has been touched. An error will be returned.
            if (!modelState.IsValid)
            {
                return new ApiBadRequestResponse(modelState);
            }

            return null;
        }
    }
}
