using System;
using System.Linq;

namespace ServiceLayer.BookServices.QueryObjects
{
    public static class BookListDtoSort
    {
        public static IQueryable<BookListDto> OrderBooksBy(this IQueryable<BookListDto> books, OrderByOptions orderByOptions)
        {
            switch (orderByOptions)
            {
                case OrderByOptions.SimpleOrder:
                    return books.OrderByDescending(x => x.BookId);
                    
                case OrderByOptions.ByVotes:
                    return books.OrderByDescending(x => x.ReviewsAverageVotes);

                case OrderByOptions.ByPublicationDate:
                    return books.OrderByDescending(x=>x.PublishedOn);

                case OrderByOptions.ByPriceLowestFirst:
                    return books.OrderBy(x => x.ActualPrice);

                case OrderByOptions.ByPriceHigestFirst:
                    return books.OrderByDescending(x => x.ActualPrice);

                default:
                    throw new ArgumentOutOfRangeException(nameof(orderByOptions), orderByOptions, null);
            }
        }
    }
}
