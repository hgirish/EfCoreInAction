using BizLogic.BasketService;
using BizLogic.Orders;
using DataLayer.EfCode;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace ServiceLayer.CheckoutServices.Concrete
{
    public  class CheckoutListService
    {
        private readonly EfCoreContext _context;
        private readonly IRequestCookieCollection _cookiesIn;

        public CheckoutListService(EfCoreContext context, IRequestCookieCollection cookiesIn)
        {
            _context = context;
            _cookiesIn = cookiesIn;
        }

        public ImmutableList<CheckoutItemDto> GetCheckoutList()
        {
            var cookieHandler = new BasketCookie(_cookiesIn);

            var service = new CheckoutCookieService(cookieHandler.GetValue());

            var lineItems = service.LineItems;

            return GetCheckoutList(lineItems);
        }

        private ImmutableList<CheckoutItemDto> GetCheckoutList(
            ImmutableList<OrderLineItem> lineItems)
        {
            var result = new List<CheckoutItemDto>();

            foreach (var item in lineItems)
            {
                result.Add(_context.Books.Select(book => new CheckoutItemDto
                {
                    BookId = book.BookId,
                    Title = book.Title,
                    AuthorsName = string.Join(", ",
                    book.AuthorsLink
                    .OrderBy(q => q.Order)
                    .Select(q => q.Author.Name)),
                    BookPrice = book.Promotion == null
                    ? book.Price
                    : book.Promotion.NewPrice,
                    ImageUrl = book.ImageUrl,
                    NumBooks = item.NumBooks
                }).Single(y => y.BookId == item.BookId));
            }
            return result.ToImmutableList();
        }
    }
}
