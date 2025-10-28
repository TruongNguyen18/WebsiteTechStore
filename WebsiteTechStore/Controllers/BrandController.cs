using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebsiteTechStore.Models;
using WebsiteTechStore.Repository;

namespace WebsiteTechStore.Controllers
{
    public class BrandController : Controller
    {
        private readonly DataContext _dataContext;
        public BrandController(DataContext context)
        {
            _dataContext = context;
        }
        public async Task<IActionResult> Index(string Slug = "", int pg = 1)
        {
            const int pageSize = 9;
            var brand = _dataContext.Brands.FirstOrDefault(b => b.Slug == Slug);
            if (brand == null)
            {
                return RedirectToAction("Index", "Home"); // giữ cách cũ gần nhất, chỉ điều hướng về Home
            }

            var query = _dataContext.Products
                                    .Where(p => p.BrandId == brand.Id)
                                    .OrderByDescending(p => p.Id);

            var totalItems = await query.CountAsync();
            if (pg < 1) pg = 1;

            // Paginate của bạn
            var pager = new Paginate(totalItems, pg, pageSize);

            // lấy dữ liệu trang hiện tại
            var items = await query.Skip((pg - 1) * pager.PageSize)
                                   .Take(pager.PageSize)
                                   .ToListAsync();

            // gửi info ra View (để không phải đổi nhiều ở View)
            ViewBag.Pager = pager;
            ViewBag.Slug = Slug;
            ViewBag.CategoryName = brand.Name;

            return View(items);
        }
    }
}
