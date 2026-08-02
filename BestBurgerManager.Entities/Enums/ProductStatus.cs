using System.ComponentModel;

namespace BestBurgerManager.Entities.Enums
{
    /// <summary>
    /// Describes the possible status of a product.
    /// </summary>
    public enum ProductStatus
    {
        [Description("In Queue")]
        Queued = 1, // We want the first status to be 1, so we define a value only for the first item.

        [Description("Being Prepared")]
        Preparing,

        [Description("Item Ready")]
        Ready,

        [Description("Item Cancelled")]
        Cancelled
    };
}
