using BizDbAccess.Orders;
using BizLogic.BasketService;
using BizLogic.Orders;
using BizLogic.Orders.Concrete;
using DataLayer.EfCode;
using Microsoft.AspNetCore.Http;
using ServiceLayer.BizRunners;
using ServiceLayer.CheckoutServices.Concrete;
using System.Collections.Immutable;
using System.ComponentModel.DataAnnotations;

namespace ServiceLayer.OrderServices.Concrete
{
    public  class PlaceOrderService
    {
        private readonly BasketCookie _basketCookie;
        private readonly RunnerWriteDb<PlaceOrderInDto, Order> _runner;
        public IImmutableList<ValidationResult> Errors => _runner.Errors;

        public PlaceOrderService(
            IRequestCookieCollection cookiesIn,
            IResponseCookies cookiesOut,
            EfCoreContext context)
        {
            _basketCookie = new BasketCookie(cookiesIn, cookiesOut);

            _runner = new RunnerWriteDb<PlaceOrderInDto, Order>(
                new PlaceOrderAction(new PlaceOrderDbAccess(context)), context);
        }

        public int PlaceOrder(bool acceptTandCs)
        {
            var checkoutService = new CheckoutCookieService(_basketCookie.GetValue());

            var order = _runner.RunAction(
                new PlaceOrderInDto(acceptTandCs,
                checkoutService.UserId,
                checkoutService.LineItems));

                if (_runner.HasErrors)
            {
                return 0;
            }
            checkoutService.ClearAllLineItems();
            _basketCookie.AddOrUpdateCookie(checkoutService.EncodeForCookie());

            return order.OrderId;
        }
    }
}
