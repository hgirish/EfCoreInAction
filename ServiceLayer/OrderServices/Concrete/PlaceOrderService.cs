using BizDbAccess.Orders;
using BizLogic.Orders;
using DataLayer.EfCode;
using Microsoft.AspNetCore.Http;
using ServiceLayer.BizRunners;
using ServiceLayer.CheckoutServices.Concrete;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.OrderServices.Concrete
{
  public  class PlaceOrderService
    {
        private readonly BasketCookie _basketCookie;
        private readonly RunnerWriteDb<PlaceOrderInDto, Order> _runner;
        public ImmutableList<ValidationResult> Errors => _runner.Errors;

        public PlaceOrderService(
            IRequestCookieCollection cookiesIn,
            IResponseCookies cookiesOut,
            EfCoreContext context)
        {
            _basketCookie = new BasketCookie(cookiesIn, cookiesOut);

            _runner = new RunnerWriteDb<PlaceOrderInDto, Order>(
                new PlaceOrderAction(new PlaceOrderDbAccess(context)), context);
        }
    }
}
