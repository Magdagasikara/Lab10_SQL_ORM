using System;
using System.Collections.Generic;

namespace Lab10_SQL_ORM.Models
{
    public partial class OrderSubtotal
    {
        public int OrderId { get; set; }
        public decimal? Subtotal { get; set; }
    }
}
