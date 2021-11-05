using System.ComponentModel.DataAnnotations.Schema;

namespace DataLayer.EfClasses
{
    public class PriceOffer 
    {
        public int PriceOfferId { get; set; }
        [Column(TypeName = "decimal(18,4)")]
        public decimal NewPrice { get; set; }
        public string PromotionalText { get; set; }    

        public int BookId { get; set; } 
    }
}
