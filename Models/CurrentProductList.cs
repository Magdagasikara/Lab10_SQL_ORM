﻿using System;
using System.Collections.Generic;

namespace Lab10_SQL_ORM.Models
{
    public partial class CurrentProductList
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = null!;
    }
}
