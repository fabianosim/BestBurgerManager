using System.Collections.Generic;
using BestBurgerManager.Entities.Enums;

namespace BestBurgerManager.Entities
{
    /// <summary>
    /// Represents an order made by a POS (Point of Sale).
    /// </summary>
    public class Order
    {
        /// <summary>
        /// The order Id.
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// The Point of Sale owner of this order. It is an user Id.
        /// </summary>
        public int PosId { get; set; }
        /// <summary>
        /// The product items ordered.
        /// </summary>
        public List<Product> Items { get; set; }
        /// <summary>
        /// The order overall status.
        /// </summary>
        public OrderStatus Status { get; set; }
    }
}
