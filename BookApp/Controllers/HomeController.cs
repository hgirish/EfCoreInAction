using DataLayer.EfCode;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServiceLayer.BookServices;
using ServiceLayer.BookServices.Concrete;
using ServiceLayer.Logger;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookApp.Controllers
{
    public class HomeController : BaseTraceController
    {
        private readonly EfCoreContext _context;

        public HomeController(EfCoreContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(SortFilterPageOptions options)
        {
            var listService = new ListBookService(_context);

            var bookList = await listService.SortFilterPage(options).ToListAsync();

            var dto = new BookListCombinedDto(options, bookList);

            SetupTraceInfo();


            return View(dto);
        }

        [HttpGet]
        public JsonResult GetFilterSearchContent(SortFilterPageOptions options)
        {
            var service = new BookFilterDropdownService(_context);

            var traceIdent = HttpContext.TraceIdentifier;

            return Json(new TraceIndentGeneric<IEnumerable<DropdownTuple>>(traceIdent, service.GetFilterDropdownValues(options.FilterBy)));


        }
    }
}
