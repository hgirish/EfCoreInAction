using Microsoft.AspNetCore.Http;

namespace ServiceLayer.CheckoutServices.Concrete
{
    public class BasketCookie : CookieTemplate
    {
        public const string BasketCookieName = "EfCoreInAction2-basket";
        public BasketCookie( IRequestCookieCollection cookiesIn, IResponseCookies cookiesOut = null) 
            : base(BasketCookieName, cookiesIn, cookiesOut)
        {
        }
        protected override int ExpireInThisManyDays => 200;
    }
}
