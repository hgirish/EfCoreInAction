using DataLayer.EfCode;
using Microsoft.EntityFrameworkCore;
using ServiceLayer.BookServices.QueryObjects;
using System;
using System.Collections.Generic;
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
    public class BookFilterDropdownService
    {
        private readonly EfCoreContext _db;

        public BookFilterDropdownService(EfCoreContext db)
        {
            _db = db;
        }
        public IEnumerable<DropdownTuple> GetFilterDropdownValues(BooksFilterBy filterBy)
        {
            switch (filterBy)
            {
                case BooksFilterBy.NoFilter:
                    return new List<DropdownTuple>();

                case BooksFilterBy.ByVotes:
                    return FormVotesDropdown();

                case BooksFilterBy.ByTags:
                    return _db.Tags
                        .Select(x => new DropdownTuple
                        {
                            Value = x.TagId,
                            Text = x.TagId
                        }).ToList();

                case BooksFilterBy.ByPublicationYear:
                    var today = DateTime.UtcNow.Date;
                    var result = _db.Books
                        .Where(x => x.PublishedOn <= today)
                        .Select(x => x.PublishedOn.Year)
                        .Distinct()
                        .OrderByDescending(x => x)
                        .Select(x => new DropdownTuple
                        {
                            Value = x.ToString(),
                            Text = x.ToString()
                        }).ToList();
                    var comingSoon = _db.Books
                        .Any(x => x.PublishedOn > today);
                    if (comingSoon)
                    {
                        result.Insert(0, new DropdownTuple
                        {
                            Value = BookListDtoFilter.AllBooksNotPublishedString,
                            Text = BookListDtoFilter.AllBooksNotPublishedString
                        });
                    }
                    return result;
                default:
                    throw new ArgumentOutOfRangeException(nameof(filterBy), filterBy, null);
            }
        }

        private IEnumerable<DropdownTuple> FormVotesDropdown()
        {
            return new[]
            {
                new DropdownTuple{Value="4", Text="4 stars and up" },
                new DropdownTuple{Value="3", Text ="3 stars and up" },
                new DropdownTuple{Value ="2", Text ="2 stars and up" },
                new DropdownTuple{Value="1", Text="1 star and up" },
            };
        }
    }
}
