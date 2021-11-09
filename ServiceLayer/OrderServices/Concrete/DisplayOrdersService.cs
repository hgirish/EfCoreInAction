using DataLayer.EfCode;
using ServiceLayer.CheckoutServices;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ServiceLayer.OrderServices.Concrete
{
    public class DisplayOrdersService
    {
        private readonly EfCoreContext _context;

        public DisplayOrdersService(EfCoreContext context)
        {
            _context = context;
        }

        public List<OrderListDto> GetUsersOrders()
        {
            return SelectQuery(_context.Orders.OrderByDescending(x => x.DateOrderedUtc)).ToList();
        }

        public OrderListDto GetOrderDetail(int orderId)
        {
            var order = SelectQuery(_context.Orders).SingleOrDefault(x => x.OrderId == orderId);

            if (order == null)
            {
                throw new NullReferenceException($"Could not find the order with id of {orderId}.");
            }
            return order;
        }

        private IQueryable<OrderListDto> SelectQuery(IQueryable<Order> orders)
        {
            return orders.Select(x => new OrderListDto
            {
                OrderId = x.OrderId,
                DateOrderedUtc = x.DateOrderedUtc,
                LineItems = x.LineItems.Select(lineItem => new CheckoutItemDto
                {
                    BookId = lineItem.BookId,
                    Title = lineItem.ChosenBook.Title,
                    ImageUrl = lineItem.ChosenBook.ImageUrl,
                    AuthorsName = string.Join(", ", 
                    lineItem.ChosenBook.AuthorsLink.OrderBy(q => q.Order)
                    .Select(q => q.Author.Name)),
                    BookPrice = lineItem.BookPrice,
                    NumBooks = lineItem.NumBooks
                })
            });
        }
    }
}
