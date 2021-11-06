using DataLayer.EfClasses;
using DataLayer.EfCode;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace ServiceLayer.AdminServices.Concrete
{
    public class ChangePriceOfferService : IChangePriceOfferService
    {
        private readonly EfCoreContext _context;

        public ChangePriceOfferService(EfCoreContext context)
        {
            _context = context;
        }

        public Book OrgBook { get; private set; }

        public ValidationResult AddRemovePriceOffer(PriceOffer promotion)
        {
            var book = _context.Books
                .Include(r => r.Promotion).Single(k => k.BookId == promotion.BookId);
            if (book.Promotion != null)
            {
                _context.Remove(book.Promotion);
                _context.SaveChanges();
                return null;
            }
            if (string.IsNullOrEmpty(promotion.PromotionalText))
            {
                return new ValidationResult(
                    "This field cnnot be empty",
                    new[] { nameof(PriceOffer.PromotionalText) });
            }
            book.Promotion = promotion;
            _context.SaveChanges();
            return null;
        }

        public PriceOffer GetOriginal(int id)
        {
            OrgBook = _context.Books
                .Include(r => r.Promotion)
                .Single(k => k.BookId == id);
            return OrgBook.Promotion ?? new PriceOffer
            {
                BookId = id,
                NewPrice = OrgBook.Price
            };
        }
    }
}
