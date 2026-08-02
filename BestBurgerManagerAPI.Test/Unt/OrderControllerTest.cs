using BestBurgerManager.Business;
using BestBurgerManager.Entities;
using BestBurgerManager.Entities.Enums;
using BestBurgerManagerAPI.ApiResponses;
using BestBurgerManagerAPI.Controllers;
using BestBurgerManagerAPI.Test.Base;
using BestBurgerManagerAPI.Test.Resources;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Xunit;

namespace BestBurgerManagerAPI.Test.Unit
{
    /// <summary>
    /// Test class for Order Controller.
    /// </summary>
    public class OrderControllerTest : ApiTestBase
    {
        /// <summary>
        /// Basic constructor.
        /// </summary>
        public OrderControllerTest()
        {
            base.Setup();
        }

        /// <summary>
        /// Tests PlaceOrder controllet method. This method places an order to the order's queue.
        /// </summary>
        [Fact]
        public void PlaceOrderTest()
        {
            Setup();

            // Calls PlaceOrder controller to enqueue a new order to the kitchen.
            IActionResult result = ordersController.PlaceOrder(JsonResources.singleOrder);

            // If the order is placed, then it will return HttpCode 200 (OkObjectResult from MVC framework).
            Assert.IsType<OkObjectResult>(result);
            Assert.IsType<OkObjectResult>(ordersController.OrderById(1));
        }

        /// <summary>
        /// Tests placing multiple orders controllet method.
        /// </summary>
        [Fact]
        public void PlaceOrdersTest()
        {
            Setup();

            // Calls PlaceOrder controller to enqueue a new order to the kitchen.
            IActionResult result = ordersController.PlaceOrders(JsonResources.multipleOrders);

            // If the orders are placed, then it will return HttpCode 200 (OkObjectResult from MVC framework).
            Assert.IsType<OkObjectResult>(result);
            Assert.IsType<OkObjectResult>(ordersController.OrderById(3)); // An order with id 3 should exists.
        }

        /// <summary>
        /// Tests CancelOrder controllet method. This method cancels an order from the queue.
        /// </summary>
        [Fact]
        public void CancelOrderTest()
        {
            Setup();

            // Calls PlaceOrder controller to enqueue a new order to the kitchen.
            IActionResult result = ordersController.PlaceOrder(JsonResources.singleOrder);
            IActionResult resultCancel = ordersController.CancelOrder(1);

            // Get the cancelled order.
            var objectResult = ordersController.OrderById(1);
            Order resultOrder = GetOrderFromResult(objectResult as OkObjectResult);

            // If the order is placed, then it will return HttpCode 200 (OkObjectResult from MVC framework).
            Assert.IsType<OkObjectResult>(result);
            Assert.IsType<OkObjectResult>(resultCancel);
            Assert.Equal(OrderStatus.Cancelled, resultOrder.Status);
        }

        /// <summary>
        /// Tests DeleteOrder controller method. This method deletes an order from the queue.
        /// </summary>
        [Fact]
        public void DeleteOrderTest()
        {
            Setup();

            // Calls PlaceOrder controller to enqueue a new order to the kitchen.
            IActionResult result = ordersController.PlaceOrder(JsonResources.singleOrder);

            // Get the created order.
            var objectResult = ordersController.OrderById(1);
            Order resultOrder = GetOrderFromResult(objectResult as OkObjectResult);

            // If the order is placed, then it will return HttpCode 200 (OkObjectResult from MVC framework).
            Assert.IsType<OkObjectResult>(result);
            Assert.NotNull(resultOrder);

            IActionResult resultDelete = ordersController.Delete(resultOrder.Id);
            Assert.IsType<OkObjectResult>(resultDelete);

            // Try to fetch the deleted order. It will 
            IActionResult resultOrderDeleted = ordersController.OrderById(1);
            Assert.IsType<BadRequestObjectResult>(resultOrderDeleted);
        }

        /// <summary>
        /// Tests OrderById controller method. This method returns a registered order after any POS places it.
        /// </summary>
        [Fact]
        public void OrderByIdTest()
        {
            Setup();

            // Insert an order in order manager service, so we can test only OrderById method.
            Order testOrder = JsonConvert.DeserializeObject<Order>(JsonResources.singleOrder);
            orderManager.PlaceOrder(testOrder);

            // Resets the orders controller in order to work with an existing order.
            SetupOrderController();


            // Get the existing order.
            var objectResult = ordersController.OrderById(1);
            Order resultOrder = GetOrderFromResult(objectResult as OkObjectResult);

            Assert.IsType<OkObjectResult>(objectResult);
            Assert.NotNull(resultOrder);
        }

        /// <summary>
        /// Tests OrderByStatus controller method. This method returns a registered order after any POS places it.
        /// </summary>
        [Fact]
        public void OrderByStatusTest()
        {
            Setup();

            // Insert an order in order manager service, so we can test only OrderById method.
            Order testOrder = JsonConvert.DeserializeObject<Order>(JsonResources.singleOrder);
            testOrder.Status = OrderStatus.Ready;
            orderManager.PlaceOrder(testOrder);

            // Resets the orders controller in order to work with an existing order.
            SetupOrderController();

            var objectResult = ordersController.OrdersByStatus((int)OrderStatus.Ready) as OkObjectResult;
            List<Order> resultOrder = GetOrdersFromResult(objectResult);

            Assert.Equal(OrderStatus.Ready, resultOrder.FirstOrDefault().Status);
        }
    }
}
