using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using BestBurgerManager.Entities;
using BestBurgerManager.Entities.Enums;
using BestBurgerManager.Interfaces;
using Moq;
using BestBurgerManager.Business;
using BestBurgerManagerAPI.Controllers;
using Microsoft.AspNetCore.Mvc;
using BestBurgerManagerAPI.ApiResponses;
using Newtonsoft.Json;

namespace BestBurgerManagerAPI.Test.Base
{
    /// <summary>
    /// Setup class base for the API. Defines mocked objects to be used in the tests.
    /// </summary>
    public class ApiTestBase
    {
        protected IUserManager userManager;
        protected IOrderManager orderManager;
        protected IProductManager productManager;
        protected OrdersController ordersController;
        protected KitchenController kitchenController;

        /// <summary>
        /// Default constructor.
        /// </summary>
        public ApiTestBase()
        { }

        #region Setup Methods

        /// <summary>
        /// Setup method for all tests.
        /// This method must be called by all tests. Otherwise, data created in some tests may affect the result of other tests.
        /// </summary>
        public virtual void Setup()
        {
            SetupUserManager();
            SetupOrderManager();
            SetupProductManager();
            SetupOrderController();
            SetupKitchenController();
        }

        /// <summary>
        /// Setup the User Manager service.
        /// </summary>
        public void SetupUserManager()
        {
            userManager = new UserManager();
            userManager.AddUser(new User() { Id = 1, Name = "TestUser", Type = UserType.PointOfSale });
        }

        /// <summary>
        /// Setup the Order Manager service.
        /// </summary>
        public void SetupOrderManager()
        {
            orderManager = new OrderManager(userManager);
        }

        /// <summary>
        /// Setup the Product Manager service.
        /// </summary>
        public void SetupProductManager()
        {
            productManager = new ProductManager(userManager, orderManager);
        }

        /// <summary>
        /// As the data objects are persisted inside the controller, we must setup it separately..
        /// </summary>
        public void SetupOrderController()
        {
            ordersController = new OrdersController(userManager, orderManager);
        }

        /// <summary>
        /// As the data objects are persisted inside the controller, we must setup it separately..
        /// </summary>
        public void SetupKitchenController()
        {
            kitchenController = new KitchenController(productManager, orderManager);
        }

        #endregion

        #region Support Methods

        /// <summary>
        /// Get a list of orders from a controller response result.
        /// </summary>
        /// <param name="okResult">The OkResult Object. In order for this method to work, the controller result must have HTTP code 200.</param>
        /// <returns>The orders from the JsonArray.</returns>
        public List<Order> GetOrdersFromResult(OkObjectResult okResult)
        {
            var result = okResult.Value as ApiResponse;
            var resultOrders = JsonConvert.DeserializeObject<List<Order>>(result.Message);

            return resultOrders;
        }

        /// <summary>
        /// Get an order from a controller response result.
        /// </summary>
        /// <param name="okResult">The OkResult object.</param>
        /// <returns>The order from a JsonObject.</returns>
        public Order GetOrderFromResult(OkObjectResult okResult)
        {
            var result = okResult.Value as ApiResponse;
            var resultOrder = JsonConvert.DeserializeObject<Order>(result.Message);

            return resultOrder;
        }

        #endregion
    }
}
