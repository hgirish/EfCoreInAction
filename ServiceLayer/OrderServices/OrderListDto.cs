﻿using ServiceLayer.CheckoutServices;
using System;
using System.Collections.Generic;

namespace ServiceLayer.OrderServices
{
    public class OrderListDto
    {
        public int OrderId { get; set; }

        public DateTime DateOrderedUtc { get; set; }

        public string OrderNumber => $"SO{OrderId:D6}";

        public IEnumerable<CheckoutItemDto> LineItems { get; set; }
    }
}
