using System;
using System.Collections.Generic;

namespace DataLayer.EfCode
{
    public class Order
    {
        public int OrderId { get; set; }
        public DateTime DateOrderedUtc { get; set; }
        public Guid CustomerId { get; set; }
        public ICollection<LineItem> LineItems { get; set; }
        public string OrderNumber => $"SO{OrderId:D6}";
        public Order()
        {
            DateOrderedUtc = DateTime.UtcNow;
        }
    }
}
