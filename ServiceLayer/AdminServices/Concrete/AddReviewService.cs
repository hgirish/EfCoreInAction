using DataLayer.EfClasses;
using DataLayer.EfCode;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace ServiceLayer.AdminServices.Concrete
{
    public class AddReviewService : IAddReviewService
    {
        private readonly EfCoreContext _context;

        public AddReviewService(EfCoreContext context)
        {
            _context = context;
        }
        public string BookTitle { get; private set; }

        public Book AddReviewToBook(Review review)
        {
            var book = _context.Books
                .Include(r => r.Reviews)
                .Single(k => k.BookId == review.BookId);
            book.Reviews.Add(review);
            _context.SaveChanges();
            return book;
        }

        public Review GetBlankReview(int id)
        {
            BookTitle = _context.Books
                .Where(p => p.BookId == id)
                .Select(p => p.Title)
                .Single();
            return new Review
            {
                BookId = id
            };
        }
    }
}
