using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BestBurgerManagerAPI.ApiResponses
{
    /// <summary>
    /// Enumeration of the possible errors that will be returned to the caller.
    /// </summary>
    public enum ErrorKeys
    {
        /// <summary>
        /// An unhandled error occurred in the application.
        /// </summary>
        UnhandledError,

        /// <summary>
        /// Invalid object type error.
        /// </summary>
        InvalidObjectType,

        /// <summary>
        /// The Point of Sale ID is invalid.
        /// </summary>
        InvalidPOSId,

        /// <summary>
        /// The Status ID is invalid.
        /// </summary>
        InvalidStatusId,

        /// <summary>
        /// There is no items in the order item's list.
        /// </summary>
        NoItemsInOrder,

        /// <summary>
        /// THe user wasn't found.
        /// </summary>
        UserNotFound,

        /// <summary>
        /// Product not found.
        /// </summary>
        ProductNotFound,

        /// <summary>
        /// Order not found.
        /// </summary>
        OrderNotFound,

        /// <summary>
        /// The order is not yet fully ready. That means there are some products being prepared.
        /// </summary>
        OrderNotFullyReady
    }
}
