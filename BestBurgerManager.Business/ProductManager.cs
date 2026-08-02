using BestBurgerManager.Entities;
using BestBurgerManager.Entities.Enums;
using BestBurgerManager.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using BestBurgerManager.Business.Extenstions.Exceptions;

namespace BestBurgerManager.Business
{
    /// <summary>
    /// Manages the products of a specific order.
    /// </summary>
    public class ProductManager : IProductManager
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        public ProductManager()
        { }

        /// <summary>
        /// Holds the reference for the injected objects used by the API to manage the products of an order.
        /// </summary>
        private readonly IUserManager _userManager;
        private readonly IOrderManager _orderManager;
        public ProductManager(IUserManager userManager, IOrderManager orderManager)
        {
            _userManager = userManager;
            _orderManager = orderManager;
        }

        /// <summary>
        /// Sets the product status of an order.
        /// </summary>
        /// <param name="idProduct">The product ID in the order items list.</param>
        /// <param name="order">The order to be updated.</param>
        /// <param name="status">The new product status.</param>
        public void SetProductStatus(int idProduct, Order order, ProductStatus status)
        {
            // Find the product to be updated in the order
            Product orderProduct = order.Items.Find(p => p.Id == idProduct);

            if (orderProduct == null)
                throw new ProductNotFoundException(string.Format("The product ID {0} was not found in order items list.", idProduct));

            orderProduct.Status = status;
        }
    }
}
