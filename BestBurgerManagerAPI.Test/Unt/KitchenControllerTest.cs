using BestBurgerManager.Entities;
using BestBurgerManager.Entities.Enums;
using BestBurgerManagerAPI.Test.Base;
using BestBurgerManagerAPI.Test.Resources;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Xunit.Sdk;

namespace BestBurgerManagerAPI.Test.Unit
{
    /// <summary>
    /// Unit test class for Kitchen Controller.
    /// </summary>
    public class KitchenControllerTest : ApiTestBase
    {
        /// <summary>
        /// Basic constructor.
        /// </summary>
        public KitchenControllerTest()
        {
            base.Setup();
        }

        /// <summary>
        /// Tests setting different product status.
        /// </summary>
        /// <param name="productStatus">The target product status to set.</param>
        [Theory]
        [InlineData(ProductStatus.Preparing)]
        [InlineData(ProductStatus.Ready)]
        [InlineData(ProductStatus.Cancelled)]
        public void SetProductStatusTest(ProductStatus productStatus)
        {
            Setup();

            // Calls PlaceOrder controller to enqueue a new order to the kitchen.
            IActionResult result = ordersController.PlaceOrder(JsonResources.singleOrder);

            // Get the enqueued order.
            var objectResult = ordersController.OrderById(1);
            Order queuedOrder = GetOrderFromResult(objectResult as OkObjectResult);

            IActionResult resultAction = null;

            switch (productStatus)
            {
                case ProductStatus.Preparing:
                    resultAction = kitchenController.PrepareProduct(queuedOrder.Id, queuedOrder.Items.FirstOrDefault().Id);
                    break;
                case ProductStatus.Ready:
                    resultAction = kitchenController.ProductReady(queuedOrder.Id, queuedOrder.Items.FirstOrDefault().Id);
                    break;
                case ProductStatus.Cancelled:
                    resultAction = kitchenController.CancelProduct(queuedOrder.Id, queuedOrder.Items.FirstOrDefault().Id);
                    break;
                default:
                    throw new XunitException("There is no valid product status to test. The valid product status to be tested are: Preparing, Ready and Cancelled.");
            }

            // Get the updated product from order's list.
            var preparedProductResult = ordersController.OrderById(1);
            Product preparedProduct = GetOrderFromResult(preparedProductResult as OkObjectResult).Items.FirstOrDefault();

            Assert.Equal(productStatus, preparedProduct.Status);
        }
        
        [Fact]
        public void SetOrderReadyTest()
        {
            // Calls PlaceOrder controller to enqueue a new order to the kitchen.
            IActionResult result = ordersController.PlaceOrder(JsonResources.singleOrder);

            // Get the enqueued order.
            var objectResult = ordersController.OrderById(1);
            Order queuedOrder = GetOrderFromResult(objectResult as OkObjectResult);

            kitchenController.ProductReady(queuedOrder.Id, queuedOrder.Items.FirstOrDefault().Id);
            kitchenController.OrderReady(queuedOrder.Id);

            // Get the updated order and check the status.
            var orderUpdatedResult = ordersController.OrderById(queuedOrder.Id);
            Order orderUpdated = GetOrderFromResult(orderUpdatedResult as OkObjectResult);

            Assert.Equal(OrderStatus.Ready, orderUpdated.Status);
        }
    }
}
