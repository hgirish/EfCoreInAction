using BizLogic.BasketService;
using BizLogic.Orders;
using DataLayer.EfCode;
using Microsoft.AspNetCore.Mvc;
using ServiceLayer.CheckoutServices.Concrete;

namespace BookApp.Controllers
{
    public class CheckoutController : BaseTraceController
    {
        private readonly EfCoreContext _context;

        public CheckoutController(EfCoreContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var listService = new CheckoutListService(_context, HttpContext.Request.Cookies);

            var list = listService.GetCheckoutList();

            SetupTraceInfo();


            return View(list);
        }

        public IActionResult Buy(OrderLineItem itemToBuy)
        {
            var cookie = new BasketCookie(
                HttpContext.Request.Cookies,
                HttpContext.Response.Cookies
                );
            var service = new CheckoutCookieService(cookie.GetValue());

            service.AddLineItem(itemToBuy);

            var cookieOutString = service.EncodeForCookie();

            cookie.AddOrUpdateCookie(cookieOutString);

            SetupTraceInfo();
            return RedirectToAction("Index");
        }
    }
}
