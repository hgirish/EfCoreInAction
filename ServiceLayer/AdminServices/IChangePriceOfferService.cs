using DataLayer.EfClasses;
using System.ComponentModel.DataAnnotations;

namespace ServiceLayer.AdminServices
{
    public interface IChangePriceOfferService
    {
        Book OrgBook { get; }
        PriceOffer GetOriginal(int id);
        ValidationResult AddRemovePriceOffer(PriceOffer promotion);
    }
}
