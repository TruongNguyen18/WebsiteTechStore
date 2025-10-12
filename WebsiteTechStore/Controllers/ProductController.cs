using Microsoft.AspNetCore.Mvc;

namespace WebsiteTechStore.Controllers
{
    public class ProductController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Details(int id)
        {
            // Logic to get product details by id can be added here
            ViewBag.ProductId = id;
            return View();
        }
    }
}
