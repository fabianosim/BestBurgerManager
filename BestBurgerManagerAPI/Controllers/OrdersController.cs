using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BestBurgerManager.Business.Extenstions.Exceptions;
using BestBurgerManager.Entities;
using BestBurgerManager.Entities.Enums;
using BestBurgerManager.Interfaces;
using BestBurgerManagerAPI.ApiResponses;
using BestBurgerManagerAPI.ApiResponses.Support;
using BestBurgerManagerAPI.Support;
using ExtensionMethods;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BestBurgerManagerAPI.Controllers
{
    /// <summary>
    /// HTTP Endpoint responsible to manage the placed orders.
    /// </summary>
    [Route("api/[controller]")]
    [Produces("application/json")]
    [ApiController]
    public class OrdersController : Controller
    {
        #region Constructors and DI structure

        /// <summary>
        /// Constants for concurrency handling for PUT and GET requests.
        /// </summary>
        const string ETAG_HEADER = "ETag";
        const string MATCH_HEADER = "If-Match";

        /// <summary>
        /// UserManager DI object
        /// </summary>
        private readonly IUserManager _userManager;
        private readonly IOrderManager _orderManager;

        /// <summary>
        /// Dependency Injection setup.
        /// </summary>
        /// <param name="userManager">User Manager Service</param>
        /// <param name="orderManager">Order Manager Service</param>
        public OrdersController(IUserManager userManager, IOrderManager orderManager)
        {
            _userManager = userManager;
            _orderManager = orderManager;
        }

        #endregion

        #region HTTP Endpoints

        /// <summary>
        /// Gets an order by its ID.
        /// </summary>
        /// <param name="orderId">The order ID to be queried.</param>
        /// <returns>An ActionResult containing a Json Object and the message.</returns>
        [HttpGet]
        [Route("orderbyid/{orderId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public IActionResult OrderById(int orderId)
        {
            Order orderResult;

            try
            {
                orderResult = _orderManager.GetOrderById(orderId);

                // HTTP Response object will be null for unit tests.
                if (Response != null)
                {
                    // Handles concurrency with hash values in Header.
                    // For unmodified objects, a 304 status will be returned.
                    var eTag = HashFactory.GetHash(orderResult);
                    Response.Headers.Add(ETAG_HEADER, eTag);

                    if (Request.Headers.ContainsKey(MATCH_HEADER) && Request.Headers[MATCH_HEADER].RemoveQuotes() == eTag)
                        return Ok(new ApiResponse(StatusCodes.Status304NotModified, "Data was not mofidied."));
                }
                
            }
            catch (Exception ex)
            {
                ApiBadRequestResponse badResponse = RequestResponses.RaiseOrderOperationError(ex, ModelState);
                return badResponse != null ? BadRequest(badResponse) : null;
            }

            return Ok(new ApiResponse(StatusCodes.Status200OK, JsonConvert.SerializeObject(orderResult)));
        }

        /// POST api/[controller]/placeorders
        /// <summary>
        /// Endpoint responsible to process multiple order requests.
        /// </summary>
        /// <remarks>
        /// Usage Example:
        ///
        ///[
        ///  {
        ///    "PosId": 1,
        ///     "Items": [
        ///     {
        ///     "Id": 1,
        ///     "Name": "Burger",
        ///     "Status": 1,
        ///     "KitchenArea": 1
        ///     }
        ///     ],
        ///     "Status": 1 
        ///  },
        ///  {
        ///    "PosId": 1,
        ///    "Items": [
        ///      {
        ///        "Id": 1,
        ///        "Name": "Fries",
        ///        "Status": 1,
        ///        "KitchenArea": 2
        ///      },
        ///      {
        ///        "Id": 2,
        ///        "Name": "Burger",
        ///        "Status": 1,
        ///        "KitchenArea": 1
        ///      }
        ///    ],
        ///    "Status": 1 
        ///  },
        ///]
        ///
        /// </remarks>
        /// <param name="orders">The list of orders to be inserted. It must be a Json Array.</param>
        /// <returns>An ActionResult containing a Json Object and the message.</returns>
        [HttpPost]
        [Route("placeorders")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public IActionResult PlaceOrders([FromBody]object orders)
        {
            try
            {
                // Test the type to ensure that the request does not bring a single object, as we are registerind multiple orders.
                if (orders.GetType() == typeof(JObject))
                {
                    ModelState.AddModelError(ErrorKeys.InvalidObjectType.ToString(), "You must send an array of orders for this endpoint.");
                    return BadRequest(new ApiBadRequestResponse(ModelState));
                }
                
                _orderManager.PlaceOrder(JsonConvert.DeserializeObject(orders.ToString(), typeof(List<Order>)) as List<Order>);
            }
            catch(Exception ex)
            {
                ApiBadRequestResponse badResponse = RequestResponses.RaiseOrderOperationError(ex, ModelState);
                return badResponse != null ? BadRequest(badResponse) : null;
            }

            return Ok(new ApiResponse(StatusCodes.Status200OK, "Orders added successfully."));
        }

        /// POST api/[controller]/placeorder
        /// <summary>
        /// Endpoint responsible to process order requests.
        /// </summary>
        /// <remarks>
        /// Usage Example:
        ///
        /// {
        ///      "PosId": 1,
        ///      "Items": [
        ///          {
        ///              "Id": 1,
        ///              "Name": "Burger",
        ///              "Status": 1,
        ///              "KitchenArea": 1
        ///          }
        ///      ],
        ///      "Status": 1 
        ///  }
        ///
        /// </remarks>
        /// <param name="order">The order to be inserted. It must be a Json Object.</param>
        /// <returns>An ActionResult containing a Json Object and the message.</returns>
        [HttpPost]
        [Route("placeorder")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public IActionResult PlaceOrder([FromBody]object order)
        {
            try
            {
                // Test the type to ensure that the request does not bring a single object, as we are registerind multiple orders.
                if (order.GetType() == typeof(JArray))
                {
                    ModelState.AddModelError(ErrorKeys.InvalidObjectType.ToString(), "You must send a single Order object for this endpoint.");
                    return BadRequest(new ApiBadRequestResponse(ModelState));
                }

                _orderManager.PlaceOrder(JsonConvert.DeserializeObject(order.ToString(), typeof(Order)) as Order);
            }
            catch (Exception ex)
            {
                ApiBadRequestResponse badResponse = RequestResponses.RaiseOrderOperationError(ex, ModelState);
                return badResponse != null ? BadRequest(badResponse) : null;
            }

            return Ok(new ApiResponse(StatusCodes.Status200OK, "Order added successfully."));
        }

        /// <summary>
        /// Get an order by Order Status.
        /// </summary>
        /// <param name="statusId">The status ID to be queried.</param>
        /// <returns>Returns the order on the requested status.</returns>
        [HttpGet]
        [Route("ordersbystatus/{statusId}")]
        [ProducesResponseType(200)]
        public IActionResult OrdersByStatus(int statusId)
        {
            IEnumerable<Order> ordersResult;

            try
            {
                ordersResult = _orderManager.GetOrdersByStatus(statusId);

                // HTTP Response object will be null for unit tests.
                if (Response != null)
                {
                    // Handles concurrency with hash values in Header.
                    // For unmodified objects, a 304 status will be returned.
                    var eTag = HashFactory.GetHash(ordersResult);
                    Response.Headers.Add(ETAG_HEADER, eTag);

                    if (Request.Headers.ContainsKey(MATCH_HEADER) && Request.Headers[MATCH_HEADER].RemoveQuotes() == eTag)
                        return Ok(new ApiResponse(StatusCodes.Status304NotModified, "Data was not mofidied."));
                }
            }
            catch (Exception ex)
            {
                ApiBadRequestResponse badResponse = RequestResponses.RaiseOrderOperationError(ex, ModelState);
                return badResponse != null ? BadRequest(badResponse) : null;
            }

            return Ok(new ApiResponse(StatusCodes.Status200OK, JsonConvert.SerializeObject(ordersResult)));
        }

        /// PUT api/[controller]/order/cancel/{orderId}
        /// <summary>
        /// PUT controller responsible for setting an order to Cancelled status
        /// </summary>
        /// <param name="orderId">The order ID to be cancelled.</param>
        /// <returns>An ActionResult containing a Json Object and the message.</returns>
        [HttpPut]
        [Route("order/cancel/{orderId}")]
        [ProducesResponseType(200)]
        public IActionResult CancelOrder(int orderId)
        {
            try
            {
                Order order = _orderManager.GetOrderById(orderId);

                // HTTP Response object will be null for unit tests.
                if (Response != null)
                {
                    // Handles concurrency with hash values in Header.
                    // If the Pre Condition was not met, send 412 code to the user.
                    var eTag = HashFactory.GetHash(order);
                    Response.Headers.Add(ETAG_HEADER, eTag);

                    if (!Request.Headers.ContainsKey(MATCH_HEADER) ||
                        Request.Headers[MATCH_HEADER].RemoveQuotes() != eTag)
                    {
                        return Ok(new ApiResponse(StatusCodes.Status412PreconditionFailed));
                    }
                }

                _orderManager.SetOrderStatus(order, OrderStatus.Cancelled);             
            }
            catch (Exception ex)
            {
                ApiBadRequestResponse badResponse = RequestResponses.RaiseOrderOperationError(ex, ModelState);
                return badResponse != null ? BadRequest(badResponse) : null;
            }

            return Ok(new ApiResponse(StatusCodes.Status200OK, string.Format("Order ID {0} is now set to {1} status successfully.", orderId, OrderStatus.Cancelled.ToString())));
        }

        /// DELETE api/[controller]/order/[/controller]
        /// <summary>
        /// DELETE controller responsible for deleting an order from the persistent list.
        /// </summary>
        /// <param name="orderId">The order ID to be deleted.</param>
        /// <returns>An ActionResult containing a Json Object and the message.</returns>
        [HttpDelete("order/{orderId}")]
        [ProducesResponseType(200)]
        public IActionResult Delete(int orderId)
        {
            try
            {
                Order order = _orderManager.GetOrderById(orderId);

                _orderManager.RemoveOrder(order);
            }
            catch (Exception ex)
            {
                ApiBadRequestResponse badResponse = RequestResponses.RaiseOrderOperationError(ex, ModelState);
                return badResponse != null ? BadRequest(badResponse) : null;
            }

            return Ok(new ApiResponse(StatusCodes.Status200OK, string.Format("Order ID {0} is now deleted permantently.", orderId)));
        }

        #endregion
    }
}
