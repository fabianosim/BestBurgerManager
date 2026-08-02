using System;
using System.Collections.Generic;
using System.Text;

namespace BestBurgerManager.Entities
{
    public class QueueItem
    {
        public int OrderId { get; set; }

        public Product Item { get; set; }
    }
}
