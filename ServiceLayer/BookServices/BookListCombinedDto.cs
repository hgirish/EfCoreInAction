using System.Collections.Generic;

namespace ServiceLayer.BookServices
{
    public class BookListCombinedDto
    {

        public SortFilterPageOptions SortFilterPageData { get; private set; }

        public BookListCombinedDto(SortFilterPageOptions sortFilterPageData, IEnumerable<BookListDto> booksList)
        {
            SortFilterPageData = sortFilterPageData;
            BooksList = booksList;
        }

        public IEnumerable<BookListDto> BooksList { get; private set; }
    }
}
