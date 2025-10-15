using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebsiteTechStore.Repository;

namespace WebsiteTechStore.Controllers
{
    public class ProductController : Controller
    {
        private readonly DataContext _dataContext;
        public ProductController(DataContext context)
        {
            _dataContext = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Details(int Id)
        {
            var product = await _dataContext.Products
                .Include(p => p.Category)
                .Include(p => p.Brand)
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == Id);

            if (product == null)
                return NotFound(); // hoặc RedirectToAction("Index")

            return View(product);
        }
    }
}
