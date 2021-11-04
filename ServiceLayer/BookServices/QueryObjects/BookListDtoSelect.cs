using DataLayer.EfClasses;
using System.Linq;

namespace ServiceLayer.BookServices.QueryObjects
{
    public static class BookListDtoSelect
    {
        public static IQueryable<BookListDto> MapBookToDto(this IQueryable<Book> books)
        {
            return books.Select(book => new BookListDto
            {
                BookId = book.BookId,
                Title = book.Title,
                Price = book.Price,
                PublishedOn = book.PublishedOn,
                ActualPrice = book.Promotion == null ? book.Price : book.Promotion.NewPrice,
                PromotionPromotionalText = book.Promotion == null ? null : book.Promotion.PromotionalText,
                AuthorsOrdered = string.Join(", ", 
                book.AuthorsLink.OrderBy(ba => ba.Order)
                .Select(ba => ba.Author.Name)),
                ReviewsCount = book.Reviews.Count,
                ReviewsAverageVotes = 
                book.Reviews.Select(review=> 
                (double?)review.NumStars).Average(),
                TagStrings = book.Tags.Select(x => x.TagId).ToArray(),
            });
        }

    }
}
