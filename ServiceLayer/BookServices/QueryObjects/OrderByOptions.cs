using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.BookServices.QueryObjects
{
    public enum OrderByOptions
    {
        [Display(Name = "sort by...")] SimpleOrder = 0,
        [Display(Name = "Votes ↑")] ByVotes,
        [Display(Name = "Publication Date ↑")] ByPublicationDate,
        [Display(Name = "Price ↓")] ByPriceLowestFirst,
        [Display(Name = "Price ↑")] ByPriceHigestFirst
    }
}
