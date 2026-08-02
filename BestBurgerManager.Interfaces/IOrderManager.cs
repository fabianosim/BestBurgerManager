using System;
using System.Collections.Generic;
using System.Text;
using BestBurgerManager.Entities;
using BestBurgerManager.Entities.Enums;

namespace BestBurgerManager.Interfaces
{
    /// <summary>
    /// Interface for Order management. 
    /// Represents methods to place an order, set its status, etc.
    /// </summary>
    public interface IOrderManager
    {
        #region Action Methods

        /// <summary>
        /// Places an order to be routed to the kitchen.
        /// </summary>
        /// <param name="order">The order object.</param>
        void PlaceOrder(Order order);

        /// <summary>
        /// Places a list of orders to be routed to the kitchen.
        /// </summary>
        /// <param name="orders"></param>
        void PlaceOrder(List<Order> orders);

        /// <summary>
        /// Change an order status.
        /// </summary>
        /// <param name="order">The order object.</param>
        /// <param name="status">The new status for the order.</param>
        void SetOrderStatus(Order order, OrderStatus status);

        /// <summary>
        /// Removes an order from the queue, so it can be delivered to the customer.
        /// </summary>
        /// <param name="order"></param>
        void RemoveOrder(Order order);

        #endregion

        #region Query Methods

        /// <summary>
        /// Fetch the orders for a given status.
        /// Status is int instead of the related Enum because it will come from user input.
        /// </summary>
        /// <param name="status">The status to be considered.</param>
        /// <returns>A list of orders filtered by status.</returns>
        IEnumerable<Order> GetOrdersByStatus(int status);

        /// <summary>
        /// Gets all orders registered.
        /// </summary>
        /// <returns>A list containing all orders.</returns>
        IEnumerable<Order> GetAllOrders();

        /// <summary>
        /// Get an order by its ID.
        /// </summary>
        /// <param name="orderId">The order ID to query.</param>
        /// <returns>Returns an order object.</returns>
        Order GetOrderById(int orderId);

        #endregion
    }
}
