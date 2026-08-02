using System.ComponentModel;

namespace BestBurgerManager.Entities.Enums
{
    /// <summary>
    /// Defines the API user's type.
    /// </summary>
    public enum UserType
    {
        [Description("Point of Sale")]
        PointOfSale = 1,

        [Description("Main Kitchen")]
        Kitchen
    }
}
