using System;
using System.Collections.Immutable;

namespace BizLogic.Orders
{
    public class PlaceOrderInDto
    {
        public PlaceOrderInDto(
            bool AcceptTandCs,
            Guid userId,
            ImmutableList<OrderLineItem> lineItems)
        {
            this.AcceptTandCs = AcceptTandCs;
            UserId = userId;
            LineItems = lineItems;
        }

        public bool AcceptTandCs { get; }
        public Guid UserId { get; }
        public ImmutableList<OrderLineItem> LineItems { get; }
    }
}
