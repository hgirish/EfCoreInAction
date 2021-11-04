using DataLayer.EfCode;
using Microsoft.EntityFrameworkCore;
using ServiceLayer.BookServices.QueryObjects;
using System.Linq;

namespace ServiceLayer.BookServices.Concrete
{
    public class ListBookService
    {
        private readonly EfCoreContext _context;

        public ListBookService(EfCoreContext context)
        {
            _context = context;
        }

        public IQueryable<BookListDto> SortFilterPage(SortFilterPageOptions options)
        {
            var booksQuery = _context.Books
                .AsNoTracking()
                .MapBookToDto()
                .OrderBooksBy(options.OrderByOptions)
                .FilterBooksBy(options.FilterBy, options.FilterValue);

            options.SetupRestOfDto(booksQuery);
            return booksQuery.Page(options.PageNum - 1, options.PageSize);
        }
    }
}
