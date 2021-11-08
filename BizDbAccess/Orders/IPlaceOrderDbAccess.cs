using DataLayer.EfClasses;
using DataLayer.EfCode;
using System.Collections.Generic;

namespace BizDbAccess.Orders
{
    public interface IPlaceOrderDbAccess
    {
        IDictionary<int, Book> FindBooksByIdsWithPriceOffers(
            IEnumerable<int> bookIds);
        void Add(Order newOrder);
    }
}
