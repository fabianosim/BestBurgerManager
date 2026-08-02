using BestBurgerManager.Business.Extenstions.Exceptions;
using BestBurgerManager.Entities;
using BestBurgerManager.Entities.Enums;
using BestBurgerManager.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BestBurgerManager.Business
{
    /// <summary>
    /// Manages the orders that arrives to the API.
    /// </summary>
    public class OrderManager : IOrderManager
    {
        /// <summary>
        /// List of currnt orders being processed.
        /// </summary>
        private List<Order> CurrentOrders { get; set; } = new List<Order>();
        private Dictionary<KitchenArea, List<QueueItem>> KitchenQueue { get; set; } = new Dictionary<KitchenArea, List<QueueItem>>();


        /// <summary>
        /// Default constructor.
        /// </summary>
        public OrderManager()
        {
        }

        /// <summary>
        /// Holds the reference for the injected object used by the API to manage the users.
        /// </summary>
        private readonly IUserManager _userManager;
        public OrderManager(IUserManager userManager)
        {
            _userManager = userManager;
        }

        /// <summary>
        /// Adds a new order to the current orders list.
        /// </summary>
        /// <param name="order">The new order object.</param>
        public void PlaceOrder(Order order)
        {
            if (order == null)
                throw new ArgumentException("Order to be inserted cannot be null.");

            AddOrderToList(order);
        }

        /// <summary>
        /// Adds many order items to the current orders list.
        /// </summary>
        /// <param name="orders">A list containing the orders.</param>
        public void PlaceOrder(List<Order> orders)
        {
            foreach (Order order in orders)
            {
                PlaceOrder(order);
            }
        }

        /// <summary>
        /// Sets the status for a specific order.
        /// </summary>
        /// <param name="order">The order to have its status changed.</param>
        /// <param name="status">The desired status.</param>
        public void SetOrderStatus(Order order, OrderStatus status)
        {
            if (order == null)
                throw new ArgumentException("Order to be updated cannot be null.");

            Order currentOrder = CurrentOrders.Find(o => o.Id == order.Id);

            if (currentOrder == null)
                throw new OrderNotFoundException(string.Format("Order ID {0} won't be updated because it was not found in orders list.", order.Id));

            currentOrder.Status = status;
        }

        /// <summary>
        /// Removes an order from order's list.
        /// </summary>
        /// <param name="order">The order to be removed.</param>
        public void RemoveOrder(Order order)
        {
            if (order == null)
                throw new ArgumentException("Order to be removed cannot be null.");

            Order currentOrder = CurrentOrders.Find(o => o.Id == order.Id);

            if (CurrentOrders.Exists(o => o.Id == order.Id))
                CurrentOrders.Remove(order);
            else
                throw new OrderNotFoundException(string.Format("Order ID {0} can't be removed as it was not found in orders list.", order.Id));
        }

        /// <summary>
        /// Gets all orders for a given status.
        /// </summary>
        /// <param name="status"> The desired status.</param>
        /// <returns>Returns a list of orders on the given status.</returns>
        public IEnumerable<Order> GetOrdersByStatus(int statusId)
        {
            if (!StatusExists(statusId))
                throw new OrderInvalidStatusException("This status does not exist.");

            IEnumerable<Order> ordersByStatus = CurrentOrders.FindAll(o => (int)o.Status == statusId);

            return ordersByStatus;
        }

        /// <summary>
        /// Gets an order by ID.
        /// </summary>
        /// <param name="orderId">The order ID to query.</param>
        /// <returns>Returns an object containing the order.</returns>
        public Order GetOrderById(int orderId)
        {
            Order order = CurrentOrders.Find(o => o.Id == orderId);

            if (order == null)
                throw new OrderNotFoundException($"Order with ID {orderId} was not found.");

            return order;
        }

        /// <summary>
        /// Gets all orders registered until now.
        /// </summary>
        /// <returns>Returns a list containing all orders.</returns>
        public IEnumerable<Order> GetAllOrders()
        {
            return CurrentOrders;
        }

        #region Support Methods

        /// <summary>
        /// Sets the order ID and add it to the orders list.
        /// </summary>
        /// <param name="order">The order to be added.</param>
        private void AddOrderToList(Order order)
        {
            // Validates the order before inserting.
            ValidateOrder(order);

            // Sets the next id for the current order to be added.
            order.Id = CurrentOrders.Count > 0 ? CurrentOrders.Select(o => o.Id).Max() + 1 : 1;
            
            PopulateKitchenQueue(order);
            CurrentOrders.Add(order);
        }

        /// <summary>
        /// Checks if an order is valid to be added.
        /// </summary>
        /// <param name="order">The order to be checked.</param>
        private void ValidateOrder(Order order)
        {
            User currentUser = _userManager.GetUserById(order.PosId);

            if (order.Items.Count() == 0)
                throw new OrderWithNoItemsException("An order must contain at least one item to be added.");

            if (order.PosId == 0)
                throw new OrderInvalidPosIdException("An order must have a valid POS id.");

            if (currentUser.Type == UserType.Kitchen)
                throw new OrderInvalidPosIdException("The order cannot be placed by the kitchen.");

            if (order.Status == 0 && StatusExists((int)order.Status)) // When this status comes from the controller, it may have any value.
                throw new OrderInvalidStatusException("An order must have a valid Status.");
        }

        private void PopulateKitchenQueue(Order order)
        {
            foreach (Product item in order.Items)
            {
                if (!KitchenQueue.ContainsKey(item.KitchenArea))
                {
                    // Instantiates the queue item list.
                    KitchenQueue.Add(item.KitchenArea, new List<QueueItem>());
                }

                AddQueueItem(item.KitchenArea, order.Id, item);
            }

        }

        /// <summary>
        /// Adds an item 
        /// </summary>
        /// <param name="area"></param>
        /// <param name="orderId"></param>
        /// <param name="item"></param>
        private void AddQueueItem(KitchenArea area, int orderId, Product item)
        {
            QueueItem newItem = new QueueItem()
            {
                Item = item,
                OrderId = orderId
            };

            KitchenQueue[area].Add(newItem);
        }


        /// <summary>
        /// Checks if a given status exists.
        /// </summary>
        /// <param name="status">The status to be checked.</param>
        /// <returns>Returns true if exists; false otherwise.</returns>
        private bool StatusExists(int status)
        {
            return Enum.IsDefined(typeof(OrderStatus), status);
        }
        #endregion
    }
}
