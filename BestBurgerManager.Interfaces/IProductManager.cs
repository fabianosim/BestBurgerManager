using BestBurgerManager.Entities;
using BestBurgerManager.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace BestBurgerManager.Interfaces
{
    /// <summary>
    /// Interface for Product Manager.
    /// Represents the methods to manipulate a product.
    /// </summary>
    public interface IProductManager
    {
        /// <summary>
        /// Set a product to a specific status for a given order.
        /// </summary>
        /// <param name="idProduct">The product id to be updated.</param>
        /// <param name="order">The order object.</param>
        /// <param name="status">The new product status.</param>
        void SetProductStatus(int idProduct, Order order, ProductStatus status);
    }
}
