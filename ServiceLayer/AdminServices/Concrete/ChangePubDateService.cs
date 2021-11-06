using DataLayer.EfClasses;
using DataLayer.EfCode;
using System;
using System.Linq;

namespace ServiceLayer.AdminServices.Concrete
{
    public class ChangePubDateService : IChangePubDateService
    {
        private readonly EfCoreContext _context;

        public ChangePubDateService(EfCoreContext context)
        {
            _context = context;
        }
        public ChangePubDateDto GetOriginal(int id)
        {
            return _context.Books
                .Select(p => new ChangePubDateDto
                {
                    BookId = p.BookId,
                    Title = p.Title,
                    PublishedOn = p.PublishedOn
                })
                .Single(k => k.BookId == id);
        }

        public Book UpdateBook(ChangePubDateDto dto)
        {
            var book = _context.Books.SingleOrDefault(
                x => x.BookId == dto.BookId);

            if (book == null)
            {
                throw new ArgumentException("Book not found");
            }
            book.PublishedOn = dto.PublishedOn;
            _context.SaveChanges();
            return book;

        }
    }
}
