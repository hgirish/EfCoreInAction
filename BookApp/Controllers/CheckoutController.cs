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


            return View();
        }
    }
}
