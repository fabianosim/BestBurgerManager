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
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BestBurgerManagerAPI.Controllers
{
    /// <summary>
    /// HTTP Endpoint for main Kitchen order and product control.
    /// </summary>
    [Route("api/[controller]")]
    [Produces("application/json")]
    [ApiController]
    public class KitchenController : Controller
    {
        #region Constructors and DI structure

        
        private readonly IProductManager _productManager;
        private readonly IOrderManager _orderManager;
        /// <summary>
        /// Dependency Injection setup.
        /// </summary>
        /// <param name="productManager">Product Manager Service</param>
        /// <param name="orderManager">Order Manager Service</param>
        public KitchenController(IProductManager productManager, IOrderManager orderManager)
        {
            _productManager = productManager;
            _orderManager = orderManager;
        }

        #endregion

        #region HTTP Endpoints

        /// PUT api/[controller]/product/preparing/{idproduct}
        /// <summary>
        /// PUT controller responsible for setting an item order to Preparing status
        /// </summary>
        /// <param name="orderId">The order ID that belongs to the product.</param>
        /// <param name="productId">The product ID to be updated.</param>
        /// <returns>An ActionResult containing a Json Object and the message.</returns>
        [HttpPut]
        [Route("order/{orderId}/product/preparing/{productId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public IActionResult PrepareProduct(int orderId, int productId)
        {
            try
            {
                Order productOrder = _orderManager.GetOrderById(orderId);

                _productManager.SetProductStatus(productId, productOrder, ProductStatus.Preparing);
                _orderManager.SetOrderStatus(productOrder, OrderStatus.InProgress); // We also need to set the order automatically to InProgress status.
            }
            catch (Exception ex)
            {
                ApiBadRequestResponse badResponse = RequestResponses.RaiseKitchenOperationError(ex, ModelState);
                return badResponse != null ? BadRequest(badResponse) : null;
            }

            return Ok(new ApiResponse(200, string.Format("Product ID {0} is now set to {1} status successfully.", productId, ProductStatus.Preparing.ToString())));
        }

        /// PUT api/[controller]/product/ready/{productId}
        /// <summary>
        /// PUT controller responsible for setting an item order to Ready status
        /// </summary>
        /// <param name="orderId">THe order ID that belongs to the product.</param>
        /// <param name="productId">The product ID to be updated.</param>
        /// <returns>An ActionResult containing a Json Object and the message.</returns>
        [HttpPut]
        [Route("order/{orderId}/product/ready/{productId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public IActionResult ProductReady(int orderId, int productId)
        {
            try
            {
                Order productOrder = _orderManager.GetOrderById(orderId);

                _productManager.SetProductStatus(productId, productOrder, ProductStatus.Ready);
                _orderManager.SetOrderStatus(productOrder, OrderStatus.InProgress); // We also need to set the order automatically to InProgress status.
            }
            catch (Exception ex)
            {
                ApiBadRequestResponse badResponse = RequestResponses.RaiseKitchenOperationError(ex, ModelState);
                return badResponse != null ? BadRequest(badResponse) : null;
            }

            return Ok(new ApiResponse(200, string.Format("Product ID {0} is now set to {1} status successfully.", productId, ProductStatus.Ready.ToString())));
        }

        /// PUT api/[controller]/product/ready/{productId}
        /// <summary>
        /// PUT controller responsible for setting an item order to Cancelled status
        /// </summary>
        /// <param name="orderId">The order ID that belongs to the product.</param>
        /// <param name="productId">The product ID to be updated.</param>
        /// <returns>An ActionResult containing a Json Object and the message.</returns>
        [HttpPut]
        [Route("order/{orderId}/product/cancel/{productId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public IActionResult CancelProduct(int orderId, int productId)
        {
            try
            {
                Order productOrder = _orderManager.GetOrderById(orderId);

                _productManager.SetProductStatus(productId, productOrder, ProductStatus.Cancelled);
                _orderManager.SetOrderStatus(productOrder, OrderStatus.InProgress); // We also need to set the order automatically to InProgress status.
            }
            catch (Exception ex)
            {
                ApiBadRequestResponse badResponse = RequestResponses.RaiseKitchenOperationError(ex, ModelState);
                return badResponse != null ? BadRequest(badResponse) : null;
            }

            return Ok(new ApiResponse(200, string.Format("Product ID {0} is now set to {1} status successfully.", productId, ProductStatus.Cancelled.ToString())));
        }

        /// PUT api/[controller]/product/preparing/{productId}
        /// <summary>
        /// PUT controller responsible for setting an order to ready after all products are ready.
        /// </summary>
        /// <param name="orderId">The order ID to be updated to Ready state.</param>
        [HttpPut]
        [Route("order/ready/{orderId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public IActionResult OrderReady(int orderId)
        {
            try
            {
                Order order = _orderManager.GetOrderById(orderId);

                // Check if all products are ready. Otherwise,the order can't be set to ready.
                if (order.Items.TrueForAll(p => p.Status == ProductStatus.Ready))
                    _orderManager.SetOrderStatus(order, OrderStatus.Ready);
                else
                    throw new OrderNotFullyReadyException("All products must be in ready state before setting the order to ready.");
            }
            catch (Exception ex)
            {
                ApiBadRequestResponse badResponse = RequestResponses.RaiseKitchenOperationError(ex, ModelState);
                return badResponse != null ? BadRequest(badResponse) : null;
            }

            return Ok(new ApiResponse(200, string.Format("Order ID {0} is now set to {1} status successfully.", orderId, OrderStatus.Ready.ToString())));
        }

        #endregion
    }
}