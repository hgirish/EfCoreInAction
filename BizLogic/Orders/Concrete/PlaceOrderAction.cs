using BizDbAccess.Orders;
using BizLogic.GenericInterfaces;
using DataLayer.EfClasses;
using DataLayer.EfCode;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLogic.Orders.Concrete
{
    public class PlaceOrderAction :
          BizActionErrors,
          IBizAction<PlaceOrderInDto, Order>
    {
        private readonly IPlaceOrderDbAccess _dbAccess;

        public PlaceOrderAction(IPlaceOrderDbAccess dbAccess)
        {
            _dbAccess = dbAccess;
        }
       

        public Order Action(PlaceOrderInDto dto)
        {
            if (!dto.AcceptTandCs)
            {
                AddError("You must accept the T&Cs to place an order");
                return null;
            }
            if (!dto.LineItems.Any())
            {
                AddError("No items in your basket");
                return null;
            }
            var booksDict = _dbAccess.FindBooksByIdsWithPriceOffers(
                dto.LineItems.Select(x => x.BookId));
            var order = new Order
            {
                CustomerId = dto.UserId,
                LineItems = FormLineItemsWithErrorChecking(dto.LineItems, booksDict)
            };

            if (!HasErrors)
            {
                _dbAccess.Add(order);
            }
            return HasErrors ? null : order;

        }

        private List<LineItem> FormLineItemsWithErrorChecking(ImmutableList<OrderLineItem> lineItems, IDictionary<int, Book> booksDict)
        {
            var result = new List<LineItem>();

            var i = 1;

            foreach (var lineItem in lineItems)
            {
                if (!booksDict.ContainsKey(lineItem.BookId))
                {
                    throw new InvalidOperationException(
                        "An order failed because book, " + 
                        $"id = {lineItem.BookId} was missing.");
                }

                var book = booksDict[lineItem.BookId];
                var bookPrice = book.Promotion?.NewPrice ?? book.Price;
                if (book.Price <= 0)
                {
                    AddError($"Sorry, the book '{book.Title}' is not for sale.");
                }
                else
                {
                    result.Add(new LineItem
                    {
                        BookPrice = bookPrice,
                        ChosenBook = book,
                        LineNum = (byte)(i++),
                        NumBooks = lineItem.NumBooks
                    });
                }
            }
            return result;
        }
    }
}
