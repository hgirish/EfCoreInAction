using BizLogic.Orders;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLogic.BasketService
{
   public class CheckoutCookieService
    {
        private List<OrderLineItem> _lineItems;

        public CheckoutCookieService(string cookieContent)
        {
            DecodeCookieString(cookieContent);
        }
        public void AddLineItem(OrderLineItem newItem)
        {
            _lineItems.Add(newItem);
        }

        public void DeleteLineItem(int itemIndex)
        {
            if (itemIndex < 0 || itemIndex > _lineItems.Count)
            {
                throw new InvalidOperationException("Couldn't find the item");
            }
            _lineItems.RemoveAt(itemIndex);
        }

        public void ClearAllLineItems()
        {
            _lineItems.Clear();
        }

        public string EncodeForCookie()
        {
            var sb = new StringBuilder();
            sb.Append(UserId.ToString("N"));
            foreach (var lineItem in _lineItems)
            {
                sb.AppendFormat(",{0},{1}", lineItem.BookId, lineItem.NumBooks);
            }
            return sb.ToString();
        }
        private void DecodeCookieString(string cookieContent)
        {
            _lineItems = new List<OrderLineItem>();

            if (cookieContent == null)
            {
                UserId = Guid.NewGuid();
                return;
            }
            var parts = cookieContent.Split(',');
            UserId = Guid.Parse(parts[0]);
            for (int i = 0; i < (parts.Length)/2; i++)
            {
                _lineItems.Add(new OrderLineItem
                {
                    BookId = int.Parse(parts[i * 2 + 1]),
                    NumBooks = short.Parse(parts[i * 2 + 2])
                });
            }
        }

        public ImmutableList<OrderLineItem> LineItems => _lineItems.ToImmutableList();

        public Guid UserId { get; private set; }
    }
}
