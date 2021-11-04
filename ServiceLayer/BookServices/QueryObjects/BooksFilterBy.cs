using System.ComponentModel.DataAnnotations;

namespace ServiceLayer.BookServices.QueryObjects
{
    public enum BooksFilterBy
    {
        [Display(Name = "All")] NoFilter = 0,
        [Display(Name = "By Votes...")] ByVotes,
        [Display(Name = "By Categories...")] ByTags,
        [Display(Name = "By Year published...")] ByPublicationYear
    }
}
