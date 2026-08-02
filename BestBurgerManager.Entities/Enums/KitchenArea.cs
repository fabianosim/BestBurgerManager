using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace BestBurgerManager.Entities.Enums
{
    /// <summary>
    /// Describes the kitchen areas where the products will be destinated.
    /// </summary>
    public enum KitchenArea
    {
        [Description("Burgers Area")]
        Burgers = 1, // Here we want the first status to be 1.

        [Description("Fries Area")]
        Fries,

        [Description("Drinks Area")]
        Drinks,

        [Description("Desserts Area")]
        Desserts,
    }
}
