using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebsiteTechStore.Models;
using WebsiteTechStore.Repository;

namespace WebsiteTechStore.Controllers
{
    public class HomeController : Controller
    {
        private readonly DataContext _dataContext;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, DataContext context)
        {
            _logger = logger;
            _dataContext = context;
        }

        public async Task<IActionResult> Index(int pg =1 , int pageSize=9)
        {
            if (pg < 1) pg = 1;

            // Query s?n ph?m
            var query = _dataContext.Products
                                    .Include(p => p.Category)
                                    .Include(p => p.Brand)
                                    .AsNoTracking()
                                    .OrderByDescending(p => p.Id);

            // Tính t?ng & t?o pager
            int totalItems = await query.CountAsync();
            var pager = new Paginate(totalItems, pg, pageSize);
            ViewBag.Pager = pager;

            // L?y d? li?u trang hi?n t?i
            int recSkip = (pg - 1) * pager.PageSize;
            var products = await query.Skip(recSkip)
                                      .Take(pager.PageSize)
                                      .ToListAsync();

            return View(products);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
