using DataLayer.EfCode;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.CheckoutServices.Concrete
{
    public  class CheckoutListService
    {
        private readonly EfCoreContext _context;
        private readonly IRequestCookieCollection _cookiesIn;

        public CheckoutListService(EfCoreContext context, IRequestCookieCollection cookiesIn)
        {
            _context = context;
            _cookiesIn = cookiesIn;
        }

        public ImmutableList<CheckoutItemDto> GetCheckoutList()
        {
            var cookieHandler = new BasketCookie(_cookiesIn);

            var service = new CheckoutCookieService(cookieHandler.GetValue());

            var lineItems = service.LineItems;

            return GetCheckoutList(lineItems);
        }
    }
}
