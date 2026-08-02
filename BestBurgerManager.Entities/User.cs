using BestBurgerManager.Entities.Enums;

namespace BestBurgerManager.Entities
{
    /// <summary>
    /// Represents an user.
    /// </summary>
    public class User
    {
        /// <summary>
        /// The POS Id number.
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// The POS name.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// The logged user type. Can be a point of sale or the main kitchen.
        /// </summary>
        public UserType Type { get; set; }
    }
}
