using DataLayer.EfCode;
using Microsoft.AspNetCore.Mvc;
using ServiceLayer.OrderServices.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookApp.Controllers
{
    public class OrdersController : BaseTraceController
    {
        private readonly EfCoreContext _context;

        public OrdersController(EfCoreContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var listService = new DisplayOrdersService(_context);
            SetupTraceInfo();
            return View(listService.GetUsersOrders());
        }

        public IActionResult ConfirmOrder(int orderId)
        {
            var detailService = new DisplayOrdersService(_context);
            SetupTraceInfo();
            return View(detailService.GetOrderDetail(orderId));
        }
    }
}
