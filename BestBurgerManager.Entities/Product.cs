using BestBurgerManager.Entities.Enums;

namespace BestBurgerManager.Entities
{
    /// <summary>
    /// Represents a product to be ordered, like a burger, fries, drink, dessert, etc.
    /// </summary>
    public class Product
    {
        /// <summary>
        /// The product Id.
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// The product name.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// The kitchen target area of the product.
        /// </summary>
        public KitchenArea KitchenArea { get; set; }
        /// <summary>
        /// The production stage in the kitchen.
        /// </summary>
        public ProductStatus Status { get; set; }
    }
}
