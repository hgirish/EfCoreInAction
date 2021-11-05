using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.EfClasses
{
    public  class Book
    {
        public int BookId { get; set; } 
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime PublishedOn { get; set; }
        public string Publisher { get; set; }
        [Column(TypeName = "decimal(18,4)")]
        public decimal Price { get; set; }
        public string ImageUrl { get; set; }

        public bool SoftDeleted { get; set; }
        public PriceOffer Promotion { get; set; } 
        public ICollection<Review> Reviews { get; set; } 

        public ICollection<Tag> Tags { get; set; } 
        public ICollection<BookAuthor> AuthorsLink { get; set; }

    }
}
