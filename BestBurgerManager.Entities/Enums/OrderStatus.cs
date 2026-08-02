using System.ComponentModel;

namespace BestBurgerManager.Entities.Enums
{
    /// <summary>
    /// Describes the possible status of an order.
    /// </summary>
    public enum OrderStatus
    {
        [Description("Order Pending")]
        Pending = 1, // Here we want the first status to be 1.

        [Description("In Progress")]
        InProgress,

        [Description("Ready to Customer")]
        Ready,

        [Description("Order Cancelled")]
        Cancelled,

        [Description("Order Removed")]
        Removed
    }
}
